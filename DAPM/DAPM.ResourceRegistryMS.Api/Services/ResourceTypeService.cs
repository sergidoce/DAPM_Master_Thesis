using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class ResourceTypeService : IResourceTypeService
    {

        private IResourceTypeRepository _resourceTypeRepository;
        private readonly ILogger<IResourceTypeService> _logger;

        public ResourceTypeService(ILogger<IResourceTypeService> logger, IResourceTypeRepository resourceTypeRepository)
        {
            _resourceTypeRepository = resourceTypeRepository;
            _logger = logger;
        }
        public Task<bool> AddResourceType(ResourceTypeDto resourceTypeDto)
        {
            var resourceType = new ResourceType
            {
                Id = resourceTypeDto.Id,
                Name = resourceTypeDto.Name,
                FileExtension = resourceTypeDto.FileExtension,
            };

            return _resourceTypeRepository.AddResourceType(resourceType);
        }

        public Task<bool> DeleteResourceType(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResourceType> GetResourceType(int id)
        {
            return await _resourceTypeRepository.GetResourceType(id);
        }

        public Task<IEnumerable<ResourceType>> GetResourceType()
        {
            throw new NotImplementedException();
        }
    }
}
