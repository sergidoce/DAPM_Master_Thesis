using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class PostPipelineRequestConsumer : IQueueConsumer<PostPipelineRequest>
    {
        
        private IOrchestratorEngine _engine;

        public PostPipelineRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(PostPipelineRequest message)
        {
            _engine.StartPostPipelineProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.Pipeline, message.Name);
            return Task.CompletedTask;
        }
    }
}
