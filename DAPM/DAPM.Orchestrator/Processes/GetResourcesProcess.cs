
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class GetResourcesProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid _repositoryId;
        private Guid? _resourceId;

        private Guid _ticketId;
        public GetResourcesProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid processId, Guid organizationId,
            Guid repositoryId, Guid? resourceId) 
            : base(engine, serviceProvider, processId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _resourceId = resourceId;

            _ticketId = ticketId;
        }

        public override void StartProcess()
        {
            var getResourcesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetResourcesMessage>>();

            var message = new GetResourcesMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
            };

            getResourcesProducer.PublishMessage(message);
        }

        public override void OnGetResourcesFromRegistryResult(GetResourcesResultMessage message)
        {

            var getResourcesProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetResourcesProcessResult>>();

            var processResultMessage = new GetResourcesProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Resources = message.Resources
            };

            getResourcesProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}
