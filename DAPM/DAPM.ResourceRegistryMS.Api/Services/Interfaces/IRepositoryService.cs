using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task<Repository> GetRepositoryById(int organizationId, int repositoryId);

        Task<IEnumerable<Repository>> GetAllRepositories();
        Task<IEnumerable<Resource>> GetResourcesOfRepository(int organizationId, int repositoryId);
        Task<bool> AddRepository(RepositoryDto resource);

        Task<bool> DeleteRepository(int organizationId, int repositoryId);
    }
}
