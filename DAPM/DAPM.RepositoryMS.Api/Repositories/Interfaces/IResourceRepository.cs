using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        Task<int> AddResource(Resource resource);
        Task<Resource> GetResourceById(int repositoryId, int resourceId);
    }
}
