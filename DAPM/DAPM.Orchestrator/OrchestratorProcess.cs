using DAPM.Orchestrator.Processes;
using DAPM.Orchestrator.Services;
using DAPM.Orchestrator.Services.Models;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator
{
    public abstract class OrchestratorProcess : IOrchestratorProcess
    {
        private IServiceProvider _serviceProvider;
        protected IServiceScope _serviceScope;
        protected Guid _ticketId;
        protected OrchestratorEngine _engine;
        protected Identity _localPeerIdentity;

        public OrchestratorProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId)
        {
            _engine = engine;
            _serviceProvider = serviceProvider;
            _ticketId = ticketId;
            _serviceScope = _serviceProvider.CreateScope();

            var identityService = _serviceScope.ServiceProvider.GetRequiredService<IIdentityService>();
            _localPeerIdentity = identityService.GetIdentity();
        }

        public abstract void StartProcess();
        public virtual void EndProcess()
        {
            _engine.DeleteProcess(_ticketId);
        }


        public virtual void OnPostRepoToRegistryResult(PostRepoToRegistryResultMessage message)
        {
            return;
        }

        public virtual void OnAddResourceToRegistryResult()
        {
            return;
        }

        public virtual void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {
            return;
        }

        public virtual void OnGetRepositoriesFromRegistryResult(GetRepositoriesResultMessage message)
        {
            return;
        }

        public virtual void OnGetResourcesFromRegistryResult(GetResourcesResultMessage message)
        {
            return;
        }

        public virtual void OnGetPipelinesFromRegistryResult()
        {
            return;
        }

        public virtual void OnPostPipelineToRepoResult(PostPipelineToRepoResultMessage message)
        {
            return;
        }

        public virtual void OnCreateRepoInRepoResult(PostRepoToRepoResultMessage message)
        {
            return;
        }

        public virtual void OnPostResourceToRepoResult(PostResourceToRepoResultMessage message)
        {
            return;
        }

        public virtual void OnPostResourceToRegistryResult(PostResourceToRegistryResultMessage message)
        {
            return;
        }

        public virtual void OnGetPipelinesFromRepoResult(GetPipelinesFromRepoResultMessage message)
        {
            return;
        }

        public virtual void OnGetPipelinesFromRegistryResult(GetPipelinesResultMessage message)
        {
            return;
        }

        public virtual void OnPostPipelineToRegistryResult(PostPipelineToRegistryResultMessage message)
        {
            return;
        }

        public virtual void OnGetResourceFilesFromRepoResult(GetResourceFilesFromRepoResultMessage message)
        {
            return;
        }

        public virtual void OnHandshakeRequestResponse(HandshakeRequestResponseMessage message)
        {
            return;
        }

        public virtual void OnRegistryUpdate(RegistryUpdateMessage message)
        {
            return;
        }

        public virtual void OnApplyRegistryUpdateResult(ApplyRegistryUpdateResult message)
        {
            return;
        }

        public virtual void OnGetEntriesFromOrgResult(GetEntriesFromOrgResult message)
        {
            return;
        }

        public virtual void OnRegistryUpdateAck(RegistryUpdateAckMessage message)
        {
            return;
        }

        public virtual void OnCreatePipelineExecutionResult(CreatePipelineExecutionResultMessage message)
        {
            return;
        }

        public virtual void OnCommandEnqueued(CommandEnqueuedMessage message)
        {
            return;
        }

        public virtual void OnGetResourceFilesFromOperatorResult(GetExecutionOutputResultMessage message)
        {
            return;
        }

        public virtual void OnPostResourceToOperatorResult(PostInputResourceResultMessage message)
        {
            return;
        }

        public virtual void OnSendResourceToPeerResult(SendResourceToPeerResultMessage message)
        {
            return;
        }

        public virtual void OnExecuteOperatorResult(ExecuteOperatorResultMessage message)
        {
            return;
        }
    }
}
