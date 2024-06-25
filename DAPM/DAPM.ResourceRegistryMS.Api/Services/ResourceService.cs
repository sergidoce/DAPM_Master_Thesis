using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class ResourceService : IResourceService
    {
        private IResourceRepository _resourceRepository;
        private IRepositoryRepository _repositoryRepository;
        private IResourceTypeRepository _resourceTypeRepository;
        private readonly ILogger<IResourceService> _logger;

        public ResourceService(ILogger<IResourceService> logger, IResourceRepository resourceRepository, IRepositoryRepository repositoryRepository, IResourceTypeRepository resourceTypeRepository) 
        {
            _resourceRepository = resourceRepository;
            _repositoryRepository = repositoryRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _logger = logger;
        }

        public async Task<Resource> GetResourceById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            return await _resourceRepository.GetResourceById(organizationId, repositoryId, resourceId);
        }

        public async Task<IEnumerable<Resource>> GetResource()
        {
            return await _resourceRepository.GetAllResources();
        }

        public async Task<Resource> AddResource(RabbitMQLibrary.Models.ResourceDTO resourceDto)
        {
            var repositoryId = resourceDto.RepositoryId;

            var resource = new Resource
            {
                Id = resourceDto.Id,
                Name = resourceDto.Name,
                RepositoryId = (Guid)repositoryId,
                PeerId = resourceDto.OrganizationId,
                ResourceType = resourceDto.Type,
            };

            await _resourceRepository.AddResource(resource);

            return resource;
        }

        public async Task<bool> DeleteResource(Guid resourceId)
        {
            return await _resourceRepository.DeleteResource(resourceId);
        }

        public async Task<IEnumerable<Resource>> GetAllResources()
        {
            return await _resourceRepository.GetAllResources();
        }
    }
}
