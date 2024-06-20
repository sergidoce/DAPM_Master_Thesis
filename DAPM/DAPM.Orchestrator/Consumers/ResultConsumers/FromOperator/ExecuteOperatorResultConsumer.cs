using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromOperator
{
    public class ExecuteOperatorResultConsumer : IQueueConsumer<ExecuteOperatorResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public ExecuteOperatorResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(ExecuteOperatorResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.TicketId);
            process.OnExecuteOperatorResult(message);

            return Task.CompletedTask;
        }

    }
}
