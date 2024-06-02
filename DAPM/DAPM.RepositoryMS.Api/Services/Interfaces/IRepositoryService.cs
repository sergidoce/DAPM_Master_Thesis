using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task<int> CreateNewResource(int repositoryId, string name, byte[] resourceFile);
        Task<Models.PostgreSQL.Pipeline> CreateNewPipeline(int repositoryId, string name, RabbitMQLibrary.Models.Pipeline pipeline);
        Task<Repository> CreateNewRepository(string name);
        Task<IEnumerable<Models.PostgreSQL.Pipeline>> GetPipelinesFromRepository(int repositoryId);
    }
}
