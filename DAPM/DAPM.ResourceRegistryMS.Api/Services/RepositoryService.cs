using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
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

        public RepositoryService(ILogger<IRepositoryService> logger, 
            IRepositoryRepository repositoryRepository, 
            IPeerRepository peerRepository,
            IResourceRepository resourceRepository)
        {
            _repositoryRepository = repositoryRepository;
            _peerRepository = peerRepository;
            _resourceRepository = resourceRepository;
            _logger = logger;
        }


        public Task<bool> DeleteRepository(int organizationId, int repositoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Repository>> GetAllRepositories()
        {
            return await _repositoryRepository.GetAllRepositories();
        }
 
        public async Task<Repository> GetRepositoryById(int organizationId, int repositoryId)
        {
            return await _repositoryRepository.GetRepositoryById(organizationId, repositoryId);
        }

        public async Task<IEnumerable<Resource>> GetResourcesOfRepository(int organizationId, int repositoryId)
        {
            return _resourceRepository.GetResourcesOfRepository(organizationId, repositoryId);
        }
    }
}
