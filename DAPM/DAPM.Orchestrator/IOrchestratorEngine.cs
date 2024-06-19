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
        public void StartCollabHandshakeResponseProcess(Guid ticketId, Identity requesterPeerIdentity);


        // Pipeline Processes
        public void StartPostPipelineProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Pipeline pipeline, string name);
        public void StartGetPipelinesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid? pipelineId);
        public void StartCreatePipelineExecutionProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid pipelineId);
        public void StartTransferDataActionProcess(Guid ticketId, TransferDataActionDTO data);
        public void StartExecuteOperatorActionProcess(Guid ticketId, ExecuteOperatorActionDTO data);
        public void StartPipelineStartCommandProcess(Guid ticketId, Guid executionId);
    }
}
