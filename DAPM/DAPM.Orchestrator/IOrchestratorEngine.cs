namespace DAPM.Orchestrator
{
    public interface IOrchestratorEngine
    {
        public OrchestratorProcess GetProcess(Guid processId);
        public void DeleteProcess(Guid processId);
        public void StartGetOrganizationProcess(Guid ticketId, int? organizationId);
        public void StartGetRepositoriesProcess(Guid ticketId, int organizationId, int? repositoryId);
        public void StartGetResourcesProcess(Guid ticketId, int organizationId, int repositoryId, int? resourceId);
        public void StartPostResourceProcess(Guid ticketId, int organizationId, int repositoryId, string name, byte[] resourceFile);
    }
}
