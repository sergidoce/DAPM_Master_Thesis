using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class PostResourceFromPeerRequestConsumer : IQueueConsumer<PostResourceFromPeerRequest>
    {
        IOrchestratorEngine _engine;
        public PostResourceFromPeerRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }
        public Task ConsumeAsync(PostResourceFromPeerRequest message)
        {
            _engine.StartPostResourceFromPeerProcess(message.TicketId, message.Resource, message.StorageMode, message.ExecutionId, message.SenderPeerIdentity);
            return Task.CompletedTask;
        }
    }
}
