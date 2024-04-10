using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IResourceTypeRepository
    {
        public Task<ResourceType> GetResourceType(int resourceTypeId);
        public Task<IEnumerable<ResourceType>> GetResourceType();
        public Task<bool> AddResourceType(ResourceType resourceType);
        public Task<bool> DeleteResourceType(int resourceTypeId);
    }
}
