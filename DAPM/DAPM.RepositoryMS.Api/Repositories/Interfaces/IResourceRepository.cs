using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        Task<Resource> AddResource(Resource resource);
        Task<Resource> GetResourceById(Guid repositoryId, Guid resourceId);
    }
}
