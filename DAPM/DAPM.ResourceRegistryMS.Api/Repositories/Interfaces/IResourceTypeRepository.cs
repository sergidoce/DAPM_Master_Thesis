using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IResourceTypeRepository
    {
        public Task<ResourceType> GetResourceType(string id);
    }
}
