using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class PostPipelineRequestConsumer : IQueueConsumer<PostPipelineRequest>
    {
        public Task ConsumeAsync(PostPipelineRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
