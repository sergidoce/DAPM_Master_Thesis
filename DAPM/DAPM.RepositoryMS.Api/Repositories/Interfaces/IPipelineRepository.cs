using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IPipelineRepository
    {
        Task<int> AddPipeline(Pipeline pipeline);
        Task<Pipeline> GetPipelineById(int repositoryId, int pipelineId);
    }
}
