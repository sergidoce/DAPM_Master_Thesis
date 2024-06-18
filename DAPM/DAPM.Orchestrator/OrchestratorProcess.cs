using DAPM.Orchestrator.Processes;
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

        public OrchestratorProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId)
        {
            _engine = engine;
            _serviceProvider = serviceProvider;
            _ticketId = ticketId;
            _serviceScope = _serviceProvider.CreateScope();
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

        public virtual void OnHandshakeAck(HandshakeAckMessage message)
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
    }
}
