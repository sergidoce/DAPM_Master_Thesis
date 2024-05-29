using DAPM.Orchestrator.Processes;

namespace DAPM.Orchestrator
{
    public class OrchestratorEngine : IOrchestratorEngine
    {

        private Dictionary<Guid, OrchestratorProcess> _processes;
        private IServiceProvider _serviceProvider;

        public OrchestratorEngine(IServiceProvider serviceProvider)
        {
            _processes = new Dictionary<Guid, OrchestratorProcess>();
            _serviceProvider = serviceProvider;
        }

        public void DeleteProcess(Guid processId)
        {
            _processes.Remove(processId);
        }

        public OrchestratorProcess GetProcess(Guid processId)
        {
            return _processes[processId];
        }

        public void StartGetOrganizationProcess(Guid ticketId, int? organizationId)
        {
            var getOrganizationProcess = new GetOrganizationsProcess(this, _serviceProvider, ticketId, organizationId);
            _processes[ticketId] = getOrganizationProcess;
            getOrganizationProcess.StartProcess();
        }

        public void StartGetRepositoriesProcess(Guid ticketId, int organizationId, int? repositoryId)
        {
            var getRepositoriesProcess = new GetRepositoriesProcess(this, _serviceProvider, ticketId, organizationId, repositoryId);
            _processes[ticketId] = getRepositoriesProcess;
            getRepositoriesProcess.StartProcess();
        }

        public void StartGetResourcesProcess(Guid ticketId, int organizationId, int repositoryId, int? resourceId)
        {
            var getResourcesProcess = new GetResourcesProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, resourceId);
            _processes[ticketId] = getResourcesProcess;
            getResourcesProcess.StartProcess();
        }

        public void StartPostResourceProcess(Guid ticketId, int organizationId, int repositoryId, string name, byte[] resourceFile)
        {
            var postResourceProcess = new PostResourceProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, name, resourceFile);
            _processes[ticketId] = postResourceProcess;
            postResourceProcess.StartProcess();
        }
    }
}
