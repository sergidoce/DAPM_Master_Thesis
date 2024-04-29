using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        public Task<Resource> GetResourceById(int id);
        public Task<IEnumerable<Resource>> GetAllResources();
        public Task<bool> AddResource(Resource resource);
        public Task<bool> DeleteResource(int resourceId);

    }
}
