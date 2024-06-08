using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Models.MongoDB;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;
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

        public RepositoryService(ILogger<RepositoryService> logger,
            IResourceRepository resourceRepository,
            IFileRepository fileRepository,
            IRepositoryRepository repositoryRepository,
            IPipelineRepository pipelineRepository)
        {
            _logger = logger;
            _resourceRepository = resourceRepository;
            _fileRepository = fileRepository;
            _repositoryRepository = repositoryRepository;
            _pipelineRepository = pipelineRepository;
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

        public async Task<Models.PostgreSQL.Resource> CreateNewResource(Guid repositoryId, string name, string resourceType, IEnumerable<FileDTO> files)
        {
            var repository = await _repositoryRepository.GetRepositoryById(repositoryId);
            var fileDTO = files.First();

            if(repository != null)
            {
                string objectId = await _fileRepository.AddFile(new MongoFile { Name = fileDTO.Name, File = fileDTO.Content });

                if(objectId != null)
                {
                    var file = new Models.PostgreSQL.File
                    {
                        Name = fileDTO.Name,
                        MongoDbFileId = objectId,
                        Extension = fileDTO.Extension
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
