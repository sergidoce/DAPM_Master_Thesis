using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class PostResourceRequestConsumer : IQueueConsumer<PostResourceRequest>
    {
        IOrchestratorEngine _engine;
        public PostResourceRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }
        public Task ConsumeAsync(PostResourceRequest message)
        {
            _engine.StartPostResourceProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.Name, message.ResourceFile);
            return Task.CompletedTask;
        }
    }
}
