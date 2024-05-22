using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IRepositoryRepository
    {
        public Task<IEnumerable<Repository>> GetAllRepositories();
        public Task<Repository> GetRepositoryById(int organizationId, int repositoryId);
        public Task<bool> AddRepository(Repository repository);
        public Task<IEnumerable<Repository>> GetRepositoriesOfOrganization(int organizationId);
    }
}
