using DAPM.Orchestrator.Services.Models;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator
{
    public interface IOrchestratorEngine
    {
        public OrchestratorProcess GetProcess(Guid processId);
        public void DeleteProcess(Guid processId);
        public void StartGetOrganizationProcess(Guid ticketId, Guid? organizationId);
        public void StartGetRepositoriesProcess(Guid ticketId, Guid organizationId, Guid? repositoryId);
        public void StartCreateRepositoryProcess(Guid ticketId, Guid organizationId, string name);
        public void StartGetResourcesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid? resourceId);
        public void StartGetResourceFilesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid resourceId);
        public void StartPostResourceProcess(Guid ticketId, Guid organizationId, Guid repositoryId, string name, string resourceType,
            FileDTO file);
        public void StartPostOperatorProcess(Guid ticketId, Guid organizationId, Guid repositoryId, string name, string resourceType,
            FileDTO sourceCodeFile, FileDTO dockerfileFile);
        public void StartCollabHandshakeProcess(Guid ticketId, string requestedPeerDomain);
        public void StartCollabHandshakeResponseProcess(Guid senderProcessId, Identity requesterPeerIdentity);
        public void StartRegistryUpdateProcess(Guid senderProcessId, RegistryUpdateDTO registryUpdate, IdentityDTO senderIdentity);

        // Pipeline Processes
        public void StartPostPipelineProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Pipeline pipeline, string name);
        public void StartGetPipelinesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid? pipelineId);
        public void StartCreatePipelineExecutionProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid pipelineId);
        public void StartTransferDataActionProcess(Guid? processId, IdentityDTO orchestratorIdentity, TransferDataActionDTO data);
        public void StartSendTransferDataActionProcess(TransferDataActionDTO data);
        public void StartExecuteOperatorActionProcess(Guid? processId, IdentityDTO orchestratorIdentity, ExecuteOperatorActionDTO data);
        public void StartSendExecuteOperatorActionProcess(ExecuteOperatorActionDTO data);
        public void StartPipelineStartCommandProcess(Guid ticketId, Guid executionId);
        public void StartPostResourceFromPeerProcess(Guid processId, ResourceDTO resource, int storageMode, Guid executionId, IdentityDTO senderIdentity);
    }
}
