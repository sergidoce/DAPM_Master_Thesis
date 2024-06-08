using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task<Models.PostgreSQL.Resource> CreateNewResource(Guid repositoryId, string name, string resourceType, IEnumerable<FileDTO> files);
        Task<Models.PostgreSQL.Pipeline> CreateNewPipeline(Guid repositoryId, string name, RabbitMQLibrary.Models.Pipeline pipeline);
        Task<Repository> CreateNewRepository(string name);
        Task<IEnumerable<Models.PostgreSQL.Pipeline>> GetPipelinesFromRepository(Guid repositoryId);
    }
}
