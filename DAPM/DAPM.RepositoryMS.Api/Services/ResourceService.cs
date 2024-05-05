using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;

namespace DAPM.RepositoryMS.Api.Services
{
    public class ResourceService : Interfaces.IResourceService
    {
        private IResourceRepository _resourceRepository;
        private ILogger<ResourceService> _logger;

        public ResourceService(ILogger<ResourceService> logger, IResourceRepository resourceRepository)
        {
            _logger = logger;
            _resourceRepository = resourceRepository;

        }

        public async Task<bool> PublishResource(Resource resource)
        {
            _logger.LogWarning("Publishing Resource");
            return await _resourceRepository.AddResource(resource);
        }

        public async Task<Resource> RetrieveResource(string resourceName)
        {
            _logger.LogWarning("Receiving Resource");
            return await _resourceRepository.RetrieveResource(resourceName);
        }
    }
}
