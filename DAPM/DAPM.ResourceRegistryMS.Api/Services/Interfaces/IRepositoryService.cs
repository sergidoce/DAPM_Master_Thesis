using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task<Repository> GetRepository(string id);

        Task<IEnumerable<Repository>> GetRepository();
        Task<bool> AddRepository(RepositoryDto resource);

        Task<bool> DeleteRepository(string id);
    }
}
