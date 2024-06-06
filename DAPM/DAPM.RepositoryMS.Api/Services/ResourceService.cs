using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;

namespace DAPM.RepositoryMS.Api.Services
{
    public class ResourceService : IResourceService
    {
        private IResourceRepository _resourceRepository;
        private ILogger<ResourceService> _logger;

        public ResourceService(ILogger<ResourceService> logger, IResourceRepository resourceRepository)
        {
            _logger = logger;
            _resourceRepository = resourceRepository;

        }

        public async Task<Resource> AddResource(Resource resource)
        {
            return await _resourceRepository.AddResource(resource);
        }

        public async Task<Resource> RetrieveResource(string resourceName)
        {
            throw new NotImplementedException();
        }
    }
}
