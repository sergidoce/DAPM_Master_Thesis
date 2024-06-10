using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IPipelineRepository
    {
        Task<Pipeline> AddPipeline(Pipeline pipeline);
        Task<Pipeline> GetPipelineById(Guid repositoryId, Guid pipelineId);
    }
}
