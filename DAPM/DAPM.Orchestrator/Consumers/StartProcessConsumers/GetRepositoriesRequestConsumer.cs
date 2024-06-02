using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class GetRepositoriesRequestConsumer : IQueueConsumer<GetRepositoriesRequest>
    {
        IOrchestratorEngine _engine;
        public GetRepositoriesRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }
        public Task ConsumeAsync(GetRepositoriesRequest message)
        {
            _engine.StartGetRepositoriesProcess(message.TicketId, message.OrganizationId, message.RepositoryId);
            return Task.CompletedTask;
        }
    }
}
