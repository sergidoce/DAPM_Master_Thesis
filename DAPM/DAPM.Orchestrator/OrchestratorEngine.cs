using DAPM.Orchestrator.Processes;
using DAPM.Orchestrator.Services.Models;
using RabbitMQLibrary.Models;
using System.Xml.Linq;

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

        public void StartCollabHandshakeProcess(Guid ticketId, string requestedPeerDomain)
        {
            var collabHandshakeProcess = new CollabHandshakeProcess(this, _serviceProvider, ticketId, requestedPeerDomain);
            _processes[ticketId] = collabHandshakeProcess;
            collabHandshakeProcess.StartProcess();
        }

        public void StartCollabHandshakeResponseProcess(Guid ticketId, Identity requesterPeerIdentity)
        {
            var registerPeerProcess = new CollabHandshakeResponseProcess(this, _serviceProvider, ticketId, requesterPeerIdentity);
            _processes[ticketId] = registerPeerProcess;
            registerPeerProcess.StartProcess();
        }

        public void StartCreateRepositoryProcess(Guid ticketId, Guid organizationId, string name)
        {
            var createRepositoryProcess = new CreateRepositoryProcess(this, _serviceProvider, ticketId, organizationId, name);
            _processes[ticketId] = createRepositoryProcess;
            createRepositoryProcess.StartProcess();
        }

        public void StartGetOrganizationProcess(Guid ticketId, Guid? organizationId)
        {
            var getOrganizationProcess = new GetOrganizationsProcess(this, _serviceProvider, ticketId, organizationId);
            _processes[ticketId] = getOrganizationProcess;
            getOrganizationProcess.StartProcess();
        }

        public void StartGetPipelinesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid? pipelineId)
        {
            var getPipelinesProcess = new GetPipelinesProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, pipelineId);
            _processes[ticketId] = getPipelinesProcess;
            getPipelinesProcess.StartProcess();
        }

        public void StartGetRepositoriesProcess(Guid ticketId, Guid organizationId, Guid? repositoryId)
        {
            var getRepositoriesProcess = new GetRepositoriesProcess(this, _serviceProvider, ticketId, organizationId, repositoryId);
            _processes[ticketId] = getRepositoriesProcess;
            getRepositoriesProcess.StartProcess();
        }

        public void StartGetResourceFilesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            var getResourceFilesProcess = new GetResourceFilesProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, resourceId);
            _processes[ticketId] = getResourceFilesProcess;
            getResourceFilesProcess.StartProcess();
        }

        public void StartGetResourcesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid? resourceId)
        {
            var getResourcesProcess = new GetResourcesProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, resourceId);
            _processes[ticketId] = getResourcesProcess;
            getResourcesProcess.StartProcess();
        }

        public void StartPostPipelineProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Pipeline pipeline, string name)
        {
            var postPipelineProcess = new PostPipelineProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, pipeline, name);
            _processes[ticketId] = postPipelineProcess;
            postPipelineProcess.StartProcess();
        }

        public void StartPostResourceProcess(Guid ticketId, Guid organizationId, Guid repositoryId, string name, string resourceType, IEnumerable<FileDTO> files)
        {
            var postResourceProcess = new PostResourceProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, name, resourceType, files);
            _processes[ticketId] = postResourceProcess;
            postResourceProcess.StartProcess();
        }

    }
}
