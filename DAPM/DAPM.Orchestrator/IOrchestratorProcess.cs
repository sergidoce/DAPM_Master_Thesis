using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;

namespace DAPM.Orchestrator
{
    public interface IOrchestratorProcess
    {
        public void StartProcess();
        public void EndProcess();
        public void OnPostRepoToRegistryResult(PostRepoToRegistryResultMessage message);
        public void OnAddResourceToRegistryResult();
        public void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message);
        public void OnGetRepositoriesFromRegistryResult(GetRepositoriesResultMessage message);
        public void OnGetResourcesFromRegistryResult(GetResourcesResultMessage message);
        public void OnGetPipelinesFromRegistryResult();
        public void OnGetPipelinesFromRepoResult(GetPipelinesFromRepoResultMessage message);
        public void OnPostResourceToRepoResult(PostResourceToRepoResultMessage message);
        public void OnPostResourceToRegistryResult(PostResourceToRegistryResultMessage message);
        public void OnPostPipelineToRepoResult(PostPipelineToRepoResultMessage message);
        public void OnCreateRepoInRepoResult(PostRepoToRepoResultMessage message);



    }
}
