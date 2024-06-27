using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IResourceService
    {
        Task<Resource> AddResource(Resource resource);
        Task<Models.PostgreSQL.File> GetResourceFiles(Guid repositoryId, Guid resourceId);
        Task<(Models.PostgreSQL.File, Models.PostgreSQL.File)> GetOperatorFiles(Guid repositoryId, Guid resourceId);
        Task<Models.PostgreSQL.Resource> GetResourceById(Guid repositoryId, Guid resourceId);
        Task<Models.PostgreSQL.Operator> GetOperatorById(Guid repositoryId, Guid resourceId);
    }
}
