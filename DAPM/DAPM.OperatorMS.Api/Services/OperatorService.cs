using DAPM.OperatorMS.Api.Services.Interfaces;
using Docker.DotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using ICSharpCode.SharpZipLib.Tar;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Microsoft.Extensions.FileProviders;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;
using System.ComponentModel;

namespace DAPM.OperatorMS.Api.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OperatorService> _logger;
        private readonly DockerClient _dockerClient;
        private IQueueProducer<RunAlgorithmMessage> _runAlgorithmMessageProducer;
        private string containerId;

        public OperatorService(ILogger<OperatorService> logger, HttpClient httpClient, DockerClient dockerClient, IQueueProducer<RunAlgorithmMessage> runAlgorithmMessageProducer)
        {
            _logger = logger;
            _httpClient = httpClient;
            _dockerClient = dockerClient;
            _runAlgorithmMessageProducer = runAlgorithmMessageProducer;
        }

        public async Task<byte[]> ExecuteMiner(IFormFile eventlog, IFormFile dockerFile, IFormFile sourceCode)
        {
            copyFileIntoVolume(eventlog);

            string imageName = await CreateDockerImage(sourceCode, dockerFile);

            bool containerStarted = await CreateDockerContainerByImageName(imageName);

            if (containerStarted)
            {
                string containerStatus = await getContainerStatus(containerId);

                while (containerStatus == "running")
                {
                    containerStatus = await getContainerStatus(containerId);
                }

                byte[] outputFileBytes = RetrieveOutputFile();

                await _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters { });
                await _dockerClient.Images.DeleteImageAsync(imageName, new ImageDeleteParameters { });

                return outputFileBytes;
            }
            else { throw new Exception("Container not created"); }
        }

        public async Task<string> CreateDockerImage(IFormFile sourceCode, IFormFile dockerFile)
        {
            string directory = Directory.GetCurrentDirectory();
            string registryPath = Path.Combine(directory, "registry");

            // Create the registry folder if it doesn't exist
            if (!Directory.Exists(registryPath))
            {
                Directory.CreateDirectory(registryPath);
            }

            string dockerfilePath = Path.Combine(registryPath, "Dockerfile");
            using (var DockerReader = new FileStream(dockerfilePath, FileMode.Create))
            {
                await dockerFile.CopyToAsync(DockerReader);
            }

            string sourceCodePath = Path.Combine(registryPath, sourceCode.FileName);
            using (var sourceCodeReader = new FileStream(sourceCodePath, FileMode.Create))
            {
                await sourceCode.CopyToAsync(sourceCodeReader);
            }

            string tarFilePath = Path.ChangeExtension(dockerfilePath, "tar");

            using (FileStream tarFileStream = File.Create(tarFilePath))
            using (TarOutputStream tarOutputStream = new TarOutputStream(tarFileStream))
            {
                // Docker TarEntry
                TarEntry dockerEntry = TarEntry.CreateTarEntry(Path.GetFileName(dockerfilePath));
                dockerEntry.Size = new FileInfo(dockerfilePath).Length;

                tarOutputStream.PutNextEntry(dockerEntry);

                using (FileStream dockerfileStream = File.OpenRead(dockerfilePath))
                {
                    dockerfileStream.CopyTo(tarOutputStream);
                }

                tarOutputStream.CloseEntry();

                // Source Code TarEntry
                TarEntry sourceCodeEntry = TarEntry.CreateTarEntry(Path.GetFileName(sourceCodePath));
                sourceCodeEntry.Size = new FileInfo(sourceCodePath).Length;

                tarOutputStream.PutNextEntry(sourceCodeEntry);

                using (FileStream sourceCodeStream = File.OpenRead(sourceCodePath))
                {
                    sourceCodeStream.CopyTo(tarOutputStream);
                }

                tarOutputStream.CloseEntry();
            };

            string imageName = sourceCode.FileName;
            string tag = "latest";

            var imageBuildParams = new ImageBuildParameters
            {
                Tags = new List<string> { $"{imageName}:{tag}" },
                Dockerfile = "Dockerfile"
            };

            IEnumerable<AuthConfig> authConfigs = Enumerable.Empty<AuthConfig>();
            IDictionary<string, string> headers = new Dictionary<string, string>();
            IProgress<JSONMessage> progress = new Progress<JSONMessage>(_ => { });

            using (var tarFileStream = File.OpenRead(tarFilePath))
            {
                try
                {
                    await _dockerClient.Images.BuildImageFromDockerfileAsync(imageBuildParams, tarFileStream, authConfigs, headers, progress);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Exception: " + ex.Message);
                }
            }


            return imageName;
        }

        public async Task<bool> CreateDockerContainerByImageName(string imageName)
        {
            var containerParameters = new CreateContainerParameters
            {
                Image = imageName,
                Name = imageName,
                ExposedPorts = new Dictionary<string, EmptyStruct> { { "8080/tcp", default } },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {
                            "8080/tcp",
                            new List<PortBinding>
                            {
                                new PortBinding
                                {
                                    HostPort = "5200",
                                }
                            }
                        }
                    },
                    Binds = new[] { $"operator-data:/app/shared" }
                },
                NetworkingConfig = new NetworkingConfig
                {
                    EndpointsConfig = new Dictionary<string, EndpointSettings>
                        {
                            { "DAPM", new EndpointSettings { } }
                        }
                }
            };

            var containerResponse = await _dockerClient.Containers.CreateContainerAsync(containerParameters);
            bool containerStarted = await _dockerClient.Containers.StartContainerAsync(containerResponse.ID, new ContainerStartParameters());

            containerId = containerResponse.ID;

            var inspectResponse = await _dockerClient.Containers.InspectContainerAsync(containerId);

            while (inspectResponse.State.Status != "running")
            {
                inspectResponse = await _dockerClient.Containers.InspectContainerAsync(containerId);
            }

            return true;
        }

        public async void copyFileIntoVolume(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);

            var sharedPath = "/app/shared";

            var newFileName = $"input{fileExtension}";

            var filePath = Path.Combine(sharedPath, newFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public async Task<string> getContainerStatus(string imageName)
        {
            var inspectResponse = await _dockerClient.Containers.InspectContainerAsync(containerId);

            return inspectResponse.State.Status;
        }

        public byte[] RetrieveOutputFile()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes("/app/shared/output.html");
            return fileBytes;
        }
    }
}