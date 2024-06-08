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
        public void StartPostResourceProcess(Guid ticketId, Guid organizationId, Guid repositoryId, string name, string resourceType, IEnumerable<FileDTO> files);
        public void StartPostPipelineProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Pipeline pipeline, string name);
        public void StartGetPipelinesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid? pipelineId);
        public void StartRegisterPeerProcess(Guid ticketId, string introductionPeerAddress, string localPeerAddress, string peerName);
    }
}
