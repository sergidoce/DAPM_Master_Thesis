using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;

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


        public virtual void OnAddRepositoryToRegistryResult()
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

        public virtual void OnPostPipelineInRepoResult()
        {
            return;
        }

        public virtual void OnCreateRepoInRepoResult()
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
    }
}
