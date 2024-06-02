using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IRepositoryRepository
    {
        Task<Repository> GetRepositoryById(int repositoryId);
        Task<Repository> CreateRepository(string name);
    }
}
