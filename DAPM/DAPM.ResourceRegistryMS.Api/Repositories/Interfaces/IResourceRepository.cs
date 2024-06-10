using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        public Task<Resource> GetResourceById(Guid organizationId, Guid repositoryId, Guid resourceId);
        public IEnumerable<Resource> GetResourcesOfRepository(Guid organizationId, Guid repositoryId);
        public Task<IEnumerable<Resource>> GetAllResources();
        public Task<bool> AddResource(Resource resource);
        public Task<bool> DeleteResource(Guid resourceId);

    }
}
