using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IResourceService
    {
        Task<Resource> AddResource(Resource resource);
        Task<Models.PostgreSQL.File> GetResourceFiles(Guid repositoryId, Guid resourceId);
        Task<Resource> RetrieveResource(string resourceName);
    }
}
