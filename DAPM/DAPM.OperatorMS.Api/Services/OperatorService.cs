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

namespace DAPM.OperatorMS.Api.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OperatorService> _logger;
        private readonly DockerClient _dockerClient;

        public OperatorService(ILogger<OperatorService> logger, HttpClient httpClient, DockerClient dockerClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _dockerClient = dockerClient;
        }

        public async Task<string> ExecuteMiner(IFormFile eventlog, IFormFile sourceCode, IFormFile dockerFile)
        {
            string imageName = await CreateDockerImage(sourceCode, dockerFile);
            
            return imageName;
        }

        public async Task<string> CreateDockerImage(IFormFile sourceCode, IFormFile dockerFile)
        {
            bool imageCreated = false;
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

            string sourceCodePath = Path.Combine(registryPath, dockerFile.FileName);
            using (var sourceCodeReader = new FileStream(sourceCodePath, FileMode.Create))
            {
                await dockerFile.CopyToAsync(sourceCodeReader);
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
                Stream buildResponse = await _dockerClient.Images.BuildImageFromDockerfileAsync(tarFileStream, imageBuildParams);
            }


            return imageName;
        }
    }
}