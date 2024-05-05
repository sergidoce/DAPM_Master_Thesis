using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using RabbitMQLibrary.Models;

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

        public async Task<Resource> GetResourceById(int organizationId, int repositoryId, int resourceId)
        {
            return await _resourceRepository.GetResourceById(organizationId, repositoryId, resourceId);
        }

        public async Task<IEnumerable<Resource>> GetResource()
        {
            return await _resourceRepository.GetAllResources();
        }

        public async Task<Resource> AddResource(ResourceDTO resourceDto)
        {
            var repositoryId = resourceDto.RepositoryId;

            var resource = new Resource
            {
                Id = resourceDto.Id,
                Name = resourceDto.Name,
                RepositoryId = repositoryId,
                PeerId = resourceDto.OrganizationId,
                ResourceTypeId = 1,
            };

            await _resourceRepository.AddResource(resource);

            return resource;
        }

        public async Task<bool> DeleteResource(int resourceId)
        {
            return await _resourceRepository.DeleteResource(resourceId);
        }

        public async Task<IEnumerable<Resource>> GetAllResources()
        {
            return await _resourceRepository.GetAllResources();
        }
    }
}
