using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator
{
    public interface IOrchestratorEngine
    {
        public OrchestratorProcess GetProcess(Guid processId);
        public void DeleteProcess(Guid processId);
        public void StartGetOrganizationProcess(Guid ticketId, int? organizationId);
        public void StartGetRepositoriesProcess(Guid ticketId, int organizationId, int? repositoryId);
        public void StartCreateRepositoryProcess(Guid ticketId, int organizationId, string name);
        public void StartGetResourcesProcess(Guid ticketId, int organizationId, int repositoryId, int? resourceId);
        public void StartPostResourceProcess(Guid ticketId, int organizationId, int repositoryId, string name, byte[] resourceFile);
        public void StartPostPipelineProcess(Guid ticketId, int organizationId, int repositoryId, Pipeline pipeline, string name);
        public void StartGetPipelinesProcess(Guid ticketId, int organizationId, int repositoryId, int? pipelineId);
    }
}
