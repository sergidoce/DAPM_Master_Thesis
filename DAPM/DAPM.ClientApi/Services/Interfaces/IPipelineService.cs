namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IPipelineService
    {
        public Guid GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId);
    }
}
