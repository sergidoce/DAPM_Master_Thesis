using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IPipelineRepository
    {
        public Task<Pipeline> AddPipeline(Pipeline pipeline);
        public Task<IEnumerable<Pipeline>> GetPipelinesFromRepository(Guid organizationId, Guid repositoryId);
        public Task<Pipeline> GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId);
    }
}
