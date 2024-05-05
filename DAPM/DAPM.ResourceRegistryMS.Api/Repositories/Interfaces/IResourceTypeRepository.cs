using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IResourceTypeRepository
    {
        public Task<ResourceType> GetResourceTypeById(int id);
        public Task<IEnumerable<ResourceType>> GetAllResourceTypes();
        public Task<bool> AddResourceType(ResourceType resourceType);
        public Task<bool> DeleteResourceType(int resourceTypeId);
    }
}
