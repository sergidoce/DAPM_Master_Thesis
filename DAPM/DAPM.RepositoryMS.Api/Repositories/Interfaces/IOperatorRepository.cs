using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IOperatorRepository
    {
        Task<Operator> AddOperator(Operator resource);
        Task<Operator> GetOperatorById(Guid repositoryId, Guid resourceId);
        Task<Models.PostgreSQL.File> GetOperatorFiles(Guid repositoryId, Guid resourceId);
    }
}
