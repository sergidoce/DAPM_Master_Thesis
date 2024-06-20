using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class ExecuteOperatorActionRequestConsumer : IQueueConsumer<ExecuteOperatorActionRequest>
    {
        IOrchestratorEngine _engine;
        public ExecuteOperatorActionRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(ExecuteOperatorActionRequest message)
        {
            _engine.StartExecuteOperatorActionProcess(message.TicketId, message.Data);
            return Task.CompletedTask;
        }
    }
}
