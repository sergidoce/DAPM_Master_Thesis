using DAPM.Orchestrator.Processes;
using DAPM.Orchestrator.Processes.PipelineActions;
using DAPM.Orchestrator.Processes.PipelineCommands;
using DAPM.Orchestrator.Services.Models;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Models;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public void StartCreatePipelineExecutionProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            var createPipelineExecutionProcess = new CreatePipelineExecutionProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, pipelineId);
            _processes[ticketId] = createPipelineExecutionProcess;
            createPipelineExecutionProcess.StartProcess();
        }

        public void StartCreateRepositoryProcess(Guid ticketId, Guid organizationId, string name)
        {
            var createRepositoryProcess = new CreateRepositoryProcess(this, _serviceProvider, ticketId, organizationId, name);
            _processes[ticketId] = createRepositoryProcess;
            createRepositoryProcess.StartProcess();
        }

        public void StartExecuteOperatorActionProcess(Guid ticketId, IdentityDTO orchestratorIdentity, ExecuteOperatorActionDTO data)
        {
            var executeOperatorActionProcess = new ExecuteOperatorActionProcess(this, _serviceProvider, ticketId, data, orchestratorIdentity);
            _processes[ticketId] = executeOperatorActionProcess;
            executeOperatorActionProcess.StartProcess();
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

        public void StartPipelineStartCommandProcess(Guid ticketId, Guid executionId)
        {
            var pipelineStartCommandProcess = new PipelineStartCommandProcess(this, _serviceProvider, ticketId, executionId);
            _processes[ticketId] = pipelineStartCommandProcess;
            pipelineStartCommandProcess.StartProcess();
        }

        public void StartPostPipelineProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Pipeline pipeline, string name)
        {
            var postPipelineProcess = new PostPipelineProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, pipeline, name);
            _processes[ticketId] = postPipelineProcess;
            postPipelineProcess.StartProcess();
        }

        public void StartPostResourceProcess(Guid ticketId, Guid organizationId, Guid repositoryId, string name, string resourceType, FileDTO file)
        {
            var postResourceProcess = new PostResourceProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, name, resourceType, file);
            _processes[ticketId] = postResourceProcess;
            postResourceProcess.StartProcess();
        }

        public void StartPostOperatorProcess(Guid ticketId, Guid organizationId, Guid repositoryId, string name, string resourceType, FileDTO sourceCodeFile,
            FileDTO dockerfileFile)
        {
            var postOperatorProcess = new PostOperatorProcess(this, _serviceProvider, ticketId, organizationId, repositoryId, name, resourceType, sourceCodeFile,
                dockerfileFile);
            _processes[ticketId] = postOperatorProcess;
            postOperatorProcess.StartProcess();
        }

        public void StartTransferDataActionProcess(Guid ticketId, IdentityDTO orchestratorIdentity, TransferDataActionDTO data)
        {
            var transferDataActionProcess = new TransferDataActionProcess(this, _serviceProvider, ticketId, data, orchestratorIdentity);
            _processes[ticketId] = transferDataActionProcess;
            transferDataActionProcess.StartProcess();
        }

        public void StartRegistryUpdateProcess(Guid ticketId, RegistryUpdateDTO registryUpdate, IdentityDTO senderIdentity)
        {
            var registryUpdateProcess = new RegistryUpdateProcess(this, _serviceProvider, ticketId, registryUpdate, senderIdentity);
            _processes[ticketId] = registryUpdateProcess;
            registryUpdateProcess.StartProcess();
        }

        public void StartPostResourceFromPeerProcess(Guid ticketId, ResourceDTO resource, int storageMode, Guid executionId, IdentityDTO senderIdentity)
        {
            var postResourceProcess = new PostResourceFromPeerProcess(this, _serviceProvider, ticketId, resource, storageMode, executionId, senderIdentity);
            _processes[ticketId] = postResourceProcess;
            postResourceProcess.StartProcess();
        }

        public void StartSendTransferDataActionProcess(Guid ticketId, TransferDataActionDTO data)
        {
            var sendTransferDataActionProcess = new SendTransferDataActionProcess(this, _serviceProvider, ticketId, data);
            _processes[ticketId] = sendTransferDataActionProcess;
            sendTransferDataActionProcess.StartProcess();
        }

        public void StartSendExecuteOperatorActionProcess(Guid ticketId, ExecuteOperatorActionDTO data)
        {
            var sendExecuteOperatorActionProcess = new SendExecuteOperatorActionProcess(this, _serviceProvider, ticketId, data);
            _processes[ticketId] = sendExecuteOperatorActionProcess;
            sendExecuteOperatorActionProcess.StartProcess();
        }
    }
}
