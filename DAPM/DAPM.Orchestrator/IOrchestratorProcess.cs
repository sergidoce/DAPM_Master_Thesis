using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Messages.Repository;

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
        public void OnGetPipelinesFromRegistryResult(GetPipelinesResultMessage message);
        public void OnGetPipelinesFromRepoResult(GetPipelinesFromRepoResultMessage message);
        public void OnPostResourceToRepoResult(PostResourceToRepoResultMessage message);
        public void OnPostResourceToOperatorResult(PostInputResourceResultMessage message);
        public void OnPostResourceToRegistryResult(PostResourceToRegistryResultMessage message);
        public void OnPostPipelineToRepoResult(PostPipelineToRepoResultMessage message);
        public void OnPostPipelineToRegistryResult(PostPipelineToRegistryResultMessage message);
        public void OnCreateRepoInRepoResult(PostRepoToRepoResultMessage message);
        public void OnGetResourceFilesFromRepoResult(GetResourceFilesFromRepoResultMessage message);
        public void OnGetResourceFilesFromOperatorResult(GetExecutionOutputResultMessage message);
        public void OnSendResourceToPeerResult(SendResourceToPeerResultMessage message);


        public void OnHandshakeRequestResponse(HandshakeRequestResponseMessage message);
        public void OnRegistryUpdate(RegistryUpdateMessage message);
        public void OnApplyRegistryUpdateResult(ApplyRegistryUpdateResult message);
        public void OnGetEntriesFromOrgResult(GetEntriesFromOrgResult message);
        public void OnRegistryUpdateAck(RegistryUpdateAckMessage message);

        public void OnCreatePipelineExecutionResult(CreatePipelineExecutionResultMessage message);
        public void OnCommandEnqueued(CommandEnqueuedMessage message);
        public void OnExecuteOperatorResult(ExecuteOperatorResultMessage message);

    }
}
