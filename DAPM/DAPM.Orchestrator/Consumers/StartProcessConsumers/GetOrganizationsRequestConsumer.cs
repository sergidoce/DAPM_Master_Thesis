using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class GetOrganizationsRequestConsumer : IQueueConsumer<GetOrganizationsRequest>
    {
        IOrchestratorEngine _engine;
        public GetOrganizationsRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(GetOrganizationsRequest message)
        {
            _engine.StartGetOrganizationProcess(message.TicketId, message.OrganizationId);
            return Task.CompletedTask;
        }
    }
}
