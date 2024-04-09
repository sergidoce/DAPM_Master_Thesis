using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        public Task<Resource> GetResource(string resourceName);
        public Task<IEnumerable<Resource>> GetResource();
        public Task<bool> AddResource(Resource resource);
        public Task<bool> DeleteResource(string resourceName);

    }
}
