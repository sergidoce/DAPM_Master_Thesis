using DAPM.OperatorMS.Api.Services.Interfaces;
using Docker.DotNet;
using ICSharpCode.SharpZipLib.Tar;
using Docker.DotNet.Models;
using System.Text.RegularExpressions;
using RabbitMQLibrary.Models;
using System.Text;

namespace DAPM.OperatorMS.Api.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly ILogger<OperatorService> _logger;
        private readonly DockerClient _dockerClient;
        private string containerId;

        public OperatorService(ILogger<OperatorService> logger, DockerClient dockerClient)
        {
            _logger = logger;
            _dockerClient = dockerClient;
            containerId = string.Empty;
        }

        public async Task<FileDTO> ExecuteMiner(Guid executionId, Guid stepId)
        {
            // initialization of path variables
            string volumeDirectoryPath = $"/app/shared/{executionId}_{ stepId}/";
            string inputFilePath = $"{volumeDirectoryPath}/input_{executionId}_{stepId}";
            string outputFilePath = $"{volumeDirectoryPath}/output_{executionId}_{stepId}";
            string dockerfilePath = $"{volumeDirectoryPath}/Dockerfile";

            string imageName = $"{executionId}_{stepId}";

            // Replace placeholders in Dockerfile
            await ReplaceDockerfilePlaceholders(dockerfilePath, inputFilePath, outputFilePath);

            // Get source- and dockerfile from volume
            Tuple<FileStream, string> sourceCodeStream = GetFileFromVolume(volumeDirectoryPath, $"sourceCode_{executionId}_{stepId}");
            Tuple<FileStream, string> dockerfileStream = GetFileFromVolume(volumeDirectoryPath, "Dockerfile");

            // Create Docker Image from dockerfile and source-code
            await CreateDockerImage(sourceCodeStream, dockerfileStream, imageName);

            // Create and start docker container from image -> "ImageName"
            bool containerStarted = await CreateDockerContainerByImageName(imageName);

            if (containerStarted)
            {
                // Wait for container to stop before move on
                string containerStatus;
                do {
                    containerStatus = await GetContainerStatus();
                    await Task.Delay(TimeSpan.FromSeconds(1));
                } while (containerStatus.ToLower() == "running");

                // Retrieve output file from docker volume
                FileDTO outputFile = new FileDTO();
                outputFile.Content = RetrieveOutputFile(volumeDirectoryPath);

                // Remove container and image
                await _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters { });
                await _dockerClient.Images.DeleteImageAsync(imageName, new ImageDeleteParameters { });

                return outputFile;
            }
            else { throw new Exception("Container not created"); }
        }

        public async Task<string> CreateDockerImage(Tuple<FileStream, string> sourceCode, Tuple<FileStream, string> dockerFile, string algorithmName)
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
                await dockerFile.Item1.CopyToAsync(DockerReader);
            }

            string sourceCodePath = Path.Combine(registryPath, sourceCode.Item2);
            using (var sourceCodeReader = new FileStream(sourceCodePath, FileMode.Create))
            {
                await sourceCode.Item1.CopyToAsync(sourceCodeReader);
            }

            string tarFilePath = Path.ChangeExtension(dockerfilePath, "tar");

            using (FileStream tarFileStream = File.Create(tarFilePath))
            using (TarOutputStream tarOutputStream = new TarOutputStream(tarFileStream, Encoding.UTF8))
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

            string imageName = algorithmName;
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

            string containerStatus;

            do
            {
                containerStatus = await GetContainerStatus();
                await Task.Delay(TimeSpan.FromSeconds(1));
            } while (containerStatus.ToLower() != "running");

            return true;
        }

        public Tuple<FileStream, string> GetFileFromVolume(string filePath, string fileName)
        {
            var file = Directory.EnumerateFiles(filePath, $"{fileName}.*").FirstOrDefault();

            if (file == null)
            {
                throw new FileNotFoundException($"File '{fileName}' not found in '{filePath}'.");
            }

            string fileNameWithExtension = Path.GetFileName(fileName);
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            return Tuple.Create(fileStream, fileNameWithExtension);
        }

        /// <summary>
        /// Retrieves and returns the current status of the Docker container
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetContainerStatus()
        {
            var inspectResponse = await _dockerClient.Containers.InspectContainerAsync(containerId);

            return inspectResponse.State.Status;
        }

        /// <summary>
        /// Retrieves the output file from the given directory path
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public byte[] RetrieveOutputFile(string directoryPath)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(directoryPath + "output.*");
            return fileBytes;
        }

        /// <summary>
        /// Replace the placeholders in the Dockerfile, "*input*" and "*output*", with the given inputPath and outputPath
        /// </summary>
        /// <param name="dockerfilePath"></param>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public async Task ReplaceDockerfilePlaceholders(string dockerfilePath, string inputPath, string outputPath) 
        {
            var dockerfileContent = File.ReadAllText(dockerfilePath);

            var inputRegex = new Regex(@"\*input\*");
            var outputRegex = new Regex(@"\*output\*");

            var newDockerfileContent = inputRegex.Replace(dockerfileContent, inputPath);
            newDockerfileContent = outputRegex.Replace(newDockerfileContent, outputPath);

            await File.WriteAllTextAsync(dockerfilePath, newDockerfileContent);
        }
    }
}