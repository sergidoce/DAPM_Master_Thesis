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

namespace DAPM.OperatorMS.Api.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OperatorService> _logger;
        private readonly DockerClient _dockerClient;
        private IQueueProducer<RunAlgorithmMessage> _runAlgorithmMessageProducer;

        public OperatorService(ILogger<OperatorService> logger, HttpClient httpClient, DockerClient dockerClient, IQueueProducer<RunAlgorithmMessage> runAlgorithmMessageProducer)
        {
            _logger = logger;
            _httpClient = httpClient;
            _dockerClient = dockerClient;
            _runAlgorithmMessageProducer = runAlgorithmMessageProducer;
        }

        public async Task<string> ExecuteMiner(IFormFile eventlog, IFormFile sourceCode, IFormFile dockerFile)
        {
            string imageName = await CreateDockerImage(sourceCode, dockerFile);

            bool containerCreated = await CreateDockerContainerByImageName(imageName);

            if (containerCreated) 
            {
                return "Container " + imageName + " created and started!";
            }

            using (var memoryStream = new MemoryStream())
            {
                eventlog.CopyTo(memoryStream);
                byte[] data = memoryStream.ToArray();

                var message = new RunAlgorithmMessage
                {
                    Eventlog = data,
                };

                _runAlgorithmMessageProducer.PublishMessage(message);
            }

                return imageName;
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

            using (var tarFileStream = File.OpenRead(tarFilePath)) 
            {
                try
                {
                    var buildResponse = await _dockerClient.Images.BuildImageFromDockerfileAsync(tarFileStream, imageBuildParams);
                    using (var streamReader = new StreamReader(buildResponse))
                    {
                        string line;
                        while ((line = await streamReader.ReadLineAsync()) != null)
                        {
                            _logger.LogInformation(line);
                        }
                    }
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
            bool containerCreated = false;

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
                    ExtraHosts = new List<string> { $"rabbitmq:172.17.0.1" }
                },
                NetworkingConfig = new NetworkingConfig
                {
                    EndpointsConfig = new Dictionary<string, EndpointSettings>
                        {
                            { "dapm_DAPM", new EndpointSettings { } }
                        }
                }
            };

            var containerResponse = await _dockerClient.Containers.CreateContainerAsync(containerParameters);
            bool containerStarted = await _dockerClient.Containers.StartContainerAsync(containerResponse.ID, new ContainerStartParameters());

            await Task.Delay(5000);

            var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters { Filters = new Dictionary<string, IDictionary<string, bool>> { { "status", new Dictionary<string, bool> { { "running", true }, { "created", true } } } } });

            foreach (var container in containers)
            {
                _logger.LogInformation(container.ID);
                if (container.Image == imageName) 
                {
                    containerCreated = true;
                    break;
                }
            }

            return containerCreated;
        }

        private string GetDockerHostIp()
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "bash";
            process.StartInfo.Arguments = "-c \"ip addr show docker0 | grep -oP '(?<=inet \\d{1,3}\\.\\d{1,3}\\.\\d{1,3\\.)\\d{1,3}'\"\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Trim();
        }
    }
}