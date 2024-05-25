namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IPipelineService
    {
        public Guid GetPipelineById(int repositoryId, int pipelineId);
    }
}
