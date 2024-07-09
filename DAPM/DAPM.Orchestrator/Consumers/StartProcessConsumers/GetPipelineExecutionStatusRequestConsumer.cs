using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class GetPipelineExecutionStatusRequestConsumer : IQueueConsumer<GetPipelineExecutionStatusRequest>
    {

        IOrchestratorEngine _engine;
        public GetPipelineExecutionStatusRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }
        public Task ConsumeAsync(GetPipelineExecutionStatusRequest message)
        {
            _engine.StartGetPipelineExecutionStatusProcess(message.TicketId, message.ExecutionId);
            return Task.CompletedTask;
        }
    }
}
