using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task<int> CreateNewResource(int repositoryId, string name, byte[] resourceFile);
        Task<int> CreateNewPipeline(int repositoryId, string name, PipelineDTO pipeline);
    }
}
