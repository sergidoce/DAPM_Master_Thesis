using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class StartPipelineExecutionRequestConsumer : IQueueConsumer<StartPipelineExecutionRequest>
    {
        public Task ConsumeAsync(StartPipelineExecutionRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
