using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromOperator
{
    public class GetResourceFilesFromOperatorResultConsumer : IQueueConsumer<GetExecutionOutputResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetResourceFilesFromOperatorResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetExecutionOutputResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.TicketId);
            process.OnGetResourceFilesFromOperatorResult(message);

            return Task.CompletedTask;
        }
    }
}
