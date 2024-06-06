using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Models.MongoDB;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;

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

        public async Task<Pipeline> CreateNewPipeline(Guid repositoryId, string name, RabbitMQLibrary.Models.Pipeline pipeline)
        {
            var pipelineJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(pipeline);

            var pipelineObject = new Pipeline
            {
                Name = name,
                RepositoryId = repositoryId,
                PipelineJson = pipelineJsonString
            };


            var createdPipeline = await _pipelineRepository.AddPipeline(pipelineObject);

            return createdPipeline;

        }

        public async Task<Resource> CreateNewResource(Guid repositoryId, string name, byte[] resourceFile)
        {
            var repository = await _repositoryRepository.GetRepositoryById(repositoryId);

            if(repository != null)
            {
                string objectId = await _fileRepository.AddFile(new MongoFile { Name = name, File = resourceFile });

                if(objectId != null)
                {
                    var file = new Models.PostgreSQL.File
                    {
                        Name = name,
                        MongoDbFileId = objectId,
                        Extension = ".csv"
                    };

                    var resource = new Resource
                    {
                        Name = name,
                        File = file,
                        Repository = repository,
                        Type = "EventLog"
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

        public Task<IEnumerable<Pipeline>> GetPipelinesFromRepository(Guid repositoryId)
        {
            throw new NotImplementedException();
        }
    }
}
