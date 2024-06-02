using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class PostRepositoryRequestConsumer : IQueueConsumer<PostRepositoryRequest>
    {
        IOrchestratorEngine _engine;

        public PostRepositoryRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(PostRepositoryRequest message)
        {
            _engine.StartCreateRepositoryProcess(message.TicketId, message.OrganizationId, message.Name);
            return Task.CompletedTask;
        }
    }
}
