using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class GetPipelinesRequestConsumer : IQueueConsumer<GetPipelinesRequest>
    {
        public Task ConsumeAsync(GetPipelinesRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
