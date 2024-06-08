using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IResourceTypeRepository
    {
        public Task<ResourceType> GetResourceTypeById(Guid id);
        public Task<IEnumerable<ResourceType>> GetAllResourceTypes();
        public Task<bool> AddResourceType(ResourceType resourceType);
        public Task<bool> DeleteResourceType(Guid resourceTypeId);
    }
}
