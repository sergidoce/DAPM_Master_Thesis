using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task<Repository> GetRepositoryById(Guid organizationId, Guid repositoryId);

        Task<IEnumerable<Repository>> GetAllRepositories();
        Task<IEnumerable<Resource>> GetResourcesOfRepository(Guid organizationId, Guid repositoryId);

        Task<bool> DeleteRepository(Guid organizationId, Guid repositoryId);
    }
}
