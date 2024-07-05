
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class GetRepositoriesProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid? _repositoryId;
        private Guid _ticketId;
        public GetRepositoriesProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid processId, Guid organizationId,
            Guid? repositoryId) 
            : base(engine, serviceProvider, processId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;

            _ticketId = ticketId;
        }

        public override void StartProcess()
        {
            var getRepositoriesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetRepositoriesMessage>>();

            var message = new GetRepositoriesMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId
            };

            getRepositoriesProducer.PublishMessage(message);
        }

        public override void OnGetRepositoriesFromRegistryResult(GetRepositoriesResultMessage message)
        {

            var getRepositoriesProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetRepositoriesProcessResult>>();

            var processResultMessage = new GetRepositoriesProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Repositories = message.Repositories
            };

            getRepositoriesProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}
