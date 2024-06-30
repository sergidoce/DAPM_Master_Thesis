using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Models.MongoDB;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using MongoDB.Bson;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Services
{
    public class RepositoryService : IRepositoryService
    {
        private ILogger<RepositoryService> _logger;
        private IResourceRepository _resourceRepository;
        private IFileRepository _fileRepository;
        private IRepositoryRepository _repositoryRepository;
        private IPipelineRepository _pipelineRepository;
        private IOperatorRepository _operatorRepository;

        public RepositoryService(ILogger<RepositoryService> logger,
            IResourceRepository resourceRepository,
            IFileRepository fileRepository,
            IRepositoryRepository repositoryRepository,
            IPipelineRepository pipelineRepository,
            IOperatorRepository operatorRepository)
        {
            _logger = logger;
            _resourceRepository = resourceRepository;
            _fileRepository = fileRepository;
            _repositoryRepository = repositoryRepository;
            _pipelineRepository = pipelineRepository;
            _operatorRepository = operatorRepository;
        }

        public async Task<Models.PostgreSQL.Pipeline> CreateNewPipeline(Guid repositoryId, string name, RabbitMQLibrary.Models.Pipeline pipeline)
        {
            var pipelineJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(pipeline);

            var pipelineObject = new Models.PostgreSQL.Pipeline
            {
                Name = name,
                RepositoryId = repositoryId,
                PipelineJson = pipelineJsonString
            };

            var createdPipeline = await _pipelineRepository.AddPipeline(pipelineObject);

            return createdPipeline;

        }

        public async Task<Models.PostgreSQL.Resource> CreateNewResource(Guid repositoryId, string name, string resourceType, FileDTO fileDto)
        {
            _logger.LogInformation($"THE REPO ID IS {repositoryId}");
            var repository = await _repositoryRepository.GetRepositoryById(repositoryId);

            if(repository != null)
            {
                string objectId = await _fileRepository.AddFile(new MongoFile { Name = fileDto.Name, File = fileDto.Content });

                if(objectId != null)
                {
                    var file = new Models.PostgreSQL.File
                    {
                        Name = fileDto.Name,
                        MongoDbFileId = objectId,
                        Extension = fileDto.Extension
                    };

                    var resource = new Models.PostgreSQL.Resource
                    {
                        Name = name,
                        File = file,
                        Repository = repository,
                        Type = resourceType
                    };

                    var newResource = await _resourceRepository.AddResource(resource);

                    return newResource;
                }
            }

            return null;
        }


        public async Task<Models.PostgreSQL.Operator> CreateNewOperator(Guid repositoryId, string name, string resourceType, FileDTO sourceCode, FileDTO dockerfile)
        {
            var repository = await _repositoryRepository.GetRepositoryById(repositoryId);

            if (repository != null)
            {
                string sourceCodeObjectId = await _fileRepository.AddFile(new MongoFile { Name = sourceCode.Name, File = sourceCode.Content });
                string dockerfileObjectId = await _fileRepository.AddFile(new MongoFile { Name = dockerfile.Name, File = dockerfile.Content });

                if (sourceCodeObjectId != null && dockerfileObjectId != null)
                {
                    var sourceCodeFile = new Models.PostgreSQL.File
                    {
                        Name = sourceCode.Name,
                        MongoDbFileId = sourceCodeObjectId,
                        Extension = sourceCode.Extension
                    };

                    var dockerfileFile = new Models.PostgreSQL.File
                    {
                        Name = dockerfile.Name,
                        MongoDbFileId = dockerfileObjectId,
                        Extension = dockerfile.Extension
                    };

                    var op = new Models.PostgreSQL.Operator
                    {
                        Name = name,
                        Repository = repository,
                        Type = resourceType,
                        DockerfileFile = dockerfileFile,
                        SourceCodeFile = sourceCodeFile,
                    };

                    var newOperator = await _operatorRepository.AddOperator(op);

                    return newOperator;
                }
            }

            return null;
        }

        public async Task<Repository> CreateNewRepository(string name)
        {
            return await _repositoryRepository.CreateRepository(name);
        }

        public Task<IEnumerable<Models.PostgreSQL.Pipeline>> GetPipelinesFromRepository(Guid repositoryId)
        {
            throw new NotImplementedException();
        }
    }
}
