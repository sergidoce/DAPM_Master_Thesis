using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ILogger<IRepositoryService> _logger;
        private IResourceRepository _resourceRepository;
        private IRepositoryRepository _repositoryRepository;
        private IPeerRepository _peerRepository;
        private IPipelineRepository _pipelineRepository;

        public RepositoryService(ILogger<IRepositoryService> logger, 
            IRepositoryRepository repositoryRepository, 
            IPeerRepository peerRepository,
            IResourceRepository resourceRepository,
            IPipelineRepository pipelineRepository)
        {
            _repositoryRepository = repositoryRepository;
            _pipelineRepository = pipelineRepository;
            _peerRepository = peerRepository;
            _resourceRepository = resourceRepository;
            _logger = logger;
        }

        public async Task<Pipeline> AddPipelineToRepository(Guid organizationId, Guid repositoryId, RabbitMQLibrary.Models.PipelineDTO pipeline)
        {
            var pipelineToInsert = new Pipeline()
            {
                Id = pipeline.Id,
                RepositoryId = pipeline.RepositoryId,
                PeerId = pipeline.OrganizationId,
                Name = pipeline.Name,
            };

            return await _pipelineRepository.AddPipeline(pipelineToInsert);
        }

        public Task<bool> DeleteRepository(Guid organizationId, Guid repositoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Repository>> GetAllRepositories()
        {
            return await _repositoryRepository.GetAllRepositories();
        }

        public async Task<IEnumerable<Pipeline>> GetPipelinesOfRepository(Guid organizationId, Guid repositoryId)
        {
            return await _pipelineRepository.GetPipelinesFromRepository(organizationId, repositoryId);
        }

        public async Task<Repository> GetRepositoryById(Guid organizationId, Guid repositoryId)
        {
            return await _repositoryRepository.GetRepositoryById(organizationId, repositoryId);
        }

        public async Task<IEnumerable<Resource>> GetResourcesOfRepository(Guid organizationId, Guid repositoryId)
        {
            return _resourceRepository.GetResourcesOfRepository(organizationId, repositoryId);
        }
    }
}
