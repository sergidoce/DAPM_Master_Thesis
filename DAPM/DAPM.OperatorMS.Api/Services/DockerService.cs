using DAPM.OperatorMS.Api.Services.Interfaces;
using Docker.DotNet;
using ICSharpCode.SharpZipLib.Tar;
using Docker.DotNet.Models;
using RabbitMQLibrary.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace DAPM.OperatorMS.Api.Services
{
    public class DockerService : IDockerService
    {
        private readonly DockerClient _dockerClient;
        private readonly ILogger<DockerService> _logger;

        public DockerService(DockerClient dockerClient, ILogger<DockerService> logger)
        {
            _dockerClient = dockerClient;
            _logger = logger;
        }

        public void PostInputResource(Guid pipelineExecutionId, ResourceDTO inputResource)
        {
            string inputFilesDirPath = $"/app/shared/{pipelineExecutionId}/InputFiles";
            string outputFilesDirPath = $"/app/shared/{pipelineExecutionId}/OutputFiles";

            try
            {
                Directory.CreateDirectory(inputFilesDirPath);
                Directory.CreateDirectory(outputFilesDirPath);
                var filePath = Path.Combine(inputFilesDirPath, $"{inputResource.Id}{inputResource.File.Extension}");
                File.WriteAllBytes(filePath, inputResource.File.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public void PostOperator(Guid pipelineExecutionId, ResourceDTO sourceCodeResource, FileDTO dockerfile)
        {
            string algorithmFilesDirPath = $"/app/shared/{pipelineExecutionId}/Algorithm/{sourceCodeResource.Id}";

            try
            {
                Directory.CreateDirectory(algorithmFilesDirPath);
                var sourceCodePath = Path.Combine(algorithmFilesDirPath, $"{sourceCodeResource.Id}{sourceCodeResource.File.Extension}");
                var dockerfilePath = Path.Combine(algorithmFilesDirPath, $"Dockerfile");
                File.WriteAllBytes(sourceCodePath, sourceCodeResource.File.Content);
                File.WriteAllBytes(dockerfilePath, dockerfile.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves the output file from directory in volume with name pipelineExecutionId
        /// </summary>
        /// <param name="pipelineExecutionId"></param>
        /// <param name="outputResourceId"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public ResourceDTO GetExecutionOutputResource(Guid pipelineExecutionId, Guid outputResourceId)
        {
            string outputFilesDirPath = $"/app/shared/{pipelineExecutionId}/OutputFiles";

            string[] files = Directory.GetFiles(outputFilesDirPath, $"{outputResourceId}.*");
            if (files.Length == 0)
            {
                throw new FileNotFoundException($"The file with id {outputResourceId} was not found.");
            }

            string outputFilePath = files[0];

            try
            {
                byte[] outputFileBytes = File.ReadAllBytes(outputFilePath);
                string extension = Path.GetExtension(outputFilePath);

                var fileDTO = new FileDTO()
                {
                    Name = outputResourceId.ToString(),
                    Content = outputFileBytes,
                    Extension = ".txt"
                };

                ResourceDTO outputResource = new ResourceDTO();
                outputResource.Id = pipelineExecutionId;
                outputResource.File = fileDTO;
            
                return outputResource;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<string> CreateDockerImage(Guid pipelineExecutionId, Guid resourceId)
        {
            string algorithmFilesDirPath = $"/app/shared/{pipelineExecutionId}/Algorithm/{resourceId}";
            string directory = Directory.GetCurrentDirectory();
            string registryPath = Path.Combine(directory, "registry");

            Tuple<FileStream, string> sourceCode = GetFileFromVolume(algorithmFilesDirPath, $"{resourceId}.py");
            Tuple<FileStream, string> dockerFile = GetFileFromVolume(algorithmFilesDirPath, "Dockerfile");

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

            string timestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
            string imageName = $"{resourceId}_{timestamp}";
            string tag = "latest";

            var imageBuildParams = new ImageBuildParameters
            {
                Tags = new List<string> { $"{imageName}:{tag}" },
                Dockerfile = "Dockerfile"
            };

            IEnumerable<AuthConfig> authConfigs = Enumerable.Empty<AuthConfig>();
            IDictionary<string, string> headers = new Dictionary<string, string>();
            IProgress<JSONMessage> progress = new Progress<JSONMessage>(e => {
                Console.WriteLine($"From: {e.From}");
                Console.WriteLine($"Status: {e.Status}");
                Console.WriteLine($"Stream: {e.Stream}");
                Console.WriteLine($"ID: {e.ID}");
                Console.WriteLine($"Progress: {e.ProgressMessage}");
                Console.WriteLine($"Error: {e.ErrorMessage}");
            });
           

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

        public async Task<string> CreateDockerContainerByImageName(string imageName)
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

            string containerId = containerResponse.ID;

            string containerStatus;

            do
            {
                containerStatus = await GetContainerStatus(containerId);
                await Task.Delay(TimeSpan.FromSeconds(1));
            } while (containerStatus.ToLower() != "running");

            return containerId;
        }

        /// <summary>
        /// Retrieves and returns the current status of the Docker container
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetContainerStatus(string containerId)
        {
            var inspectResponse = await _dockerClient.Containers.InspectContainerAsync(containerId);

            return inspectResponse.State.Status;
        }

        public Tuple<FileStream, string> GetFileFromVolume(string filePath, string fileName)
        {
            string[] files = Directory.GetFiles(filePath, $"{fileName}.*");
            if (files.Length == 0)
            {
                throw new FileNotFoundException($"File '{fileName}' not found in '{filePath}'.");
            }

            string file = files[0];

            string fileNameWithExtension = Path.GetFileName(fileName);
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            return Tuple.Create(fileStream, fileNameWithExtension);
        }

        public async Task ReplaceDockerfilePlaceholders(Guid pipelineExecutionId, Guid outputResourceId, Guid operatorId, List<Guid> inputResourceIds)
        {
            string inputFilesPath = $"/app/shared/{pipelineExecutionId}/InputFiles";
            string outputFilePath = $"/app/shared/{pipelineExecutionId}/OutputFiles/{outputResourceId}.*";
            string dockerfilePath = $"/app/shared/{pipelineExecutionId}/Algorithm/{operatorId}/Dockerfile";

            string dockerfileContent = File.ReadAllText(dockerfilePath);

            if (inputResourceIds.Count == 1)
            {
                var inputRegex = new Regex(@"\*input\*");
                dockerfileContent = inputRegex.Replace(dockerfileContent, Path.Combine(inputFilesPath, inputResourceIds[0].ToString()));
            }
            else if (inputResourceIds.Count == 2)
            {
                var inputRegex1 = new Regex(@"\*input1\*");
                var inputRegex2 = new Regex(@"\*input2\*");
                dockerfileContent = inputRegex1.Replace(dockerfileContent, Path.Combine(inputFilesPath, inputResourceIds[0].ToString()));
                dockerfileContent = inputRegex2.Replace(dockerfileContent, Path.Combine(inputFilesPath, inputResourceIds[1].ToString()));
            }
            else
            {
                throw new InvalidOperationException("There must be one or two input files.");
            }

            var outputRegex = new Regex(@"\*output\*");
            dockerfileContent = outputRegex.Replace(dockerfileContent, outputFilePath);

            var algorithmRegex = new Regex(@"operator\.py");
            dockerfileContent = algorithmRegex.Replace(dockerfileContent, $"{operatorId}.py");

            await File.WriteAllTextAsync(dockerfilePath, dockerfileContent);
        }

        public void RemoveImageAndContainer(string imageName, string containerId) 
        {
            _dockerClient.Images.DeleteImageAsync(imageName, new ImageDeleteParameters{});
            _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters{});
        }
    }
}
