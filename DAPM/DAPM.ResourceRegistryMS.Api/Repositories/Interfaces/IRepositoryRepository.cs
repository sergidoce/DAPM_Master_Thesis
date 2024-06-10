using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IRepositoryRepository
    {
        public Task<IEnumerable<Repository>> GetAllRepositories();
        public Task<Repository> GetRepositoryById(Guid organizationId, Guid repositoryId);
        public Task<Repository> PostRepository(Repository repository);
        public Task<IEnumerable<Repository>> GetRepositoriesOfOrganization(Guid organizationId);
    }
}
