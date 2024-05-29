using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class GetResourcesRequestConsumer : IQueueConsumer<GetResourcesRequest>
    {
        IOrchestratorEngine _engine;
        public GetResourcesRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }
        public Task ConsumeAsync(GetResourcesRequest message)
        {
            _engine.StartGetResourcesProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.ResourceId);
            return Task.CompletedTask;
        }
    }
}
