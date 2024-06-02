namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IPipelineService
    {
        public Guid GetPipelineById(int organizationId, int repositoryId, int pipelineId);
    }
}
