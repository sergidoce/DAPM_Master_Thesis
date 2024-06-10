using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class GetResourceFilesRequestConsumer : IQueueConsumer<GetResourceFilesRequest>
    {
        IOrchestratorEngine _engine;
        public GetResourceFilesRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }
        public Task ConsumeAsync(GetResourceFilesRequest message)
        {
            _engine.StartGetResourceFilesProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.ResourceId);
            return Task.CompletedTask;
        }
    }
}
