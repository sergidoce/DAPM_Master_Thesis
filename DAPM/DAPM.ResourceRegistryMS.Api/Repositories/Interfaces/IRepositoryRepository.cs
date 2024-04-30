using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IRepositoryRepository
    {
        public Task<Repository> GetRepositoryById(int id);
        public Task<bool> AddRepository(Repository repository);
        public Task<IEnumerable<Repository>> GetRepositoriesOfOrganization(int organizationId);
    }
}
