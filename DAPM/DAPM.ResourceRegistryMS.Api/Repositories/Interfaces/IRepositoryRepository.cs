using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IRepositoryRepository
    {
        public Task<Repository> GetRepository(int id);
        public Task<bool> AddRepository(Repository repository);
    }
}
