using DAPM.Orchestrator.Consumers.ResultConsumers;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class GetOrganizationsProcess : OrchestratorProcess
    {
        private Guid? _organizationId;


        public GetOrganizationsProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid? organizationId) : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
        }

        public override void StartProcess()
        {
            var getOrganizationsProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOrganizationsMessage>>();

            var message = new GetOrganizationsMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId
            };

            getOrganizationsProducer.PublishMessage(message);
        }

        public override void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {

            var getOrganizationsProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOrganizationsProcessResult>>();

            var processResultMessage = new GetOrganizationsProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Organizations = message.Organizations
            };

            getOrganizationsProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}
