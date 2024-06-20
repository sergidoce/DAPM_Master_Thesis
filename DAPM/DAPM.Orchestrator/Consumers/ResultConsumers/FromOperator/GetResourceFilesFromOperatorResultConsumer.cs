using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromOperator
{
    public class GetResourceFilesFromOperatorResultConsumer : IQueueConsumer<GetResourceFilesFromOperatorResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetResourceFilesFromOperatorResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetResourceFilesFromOperatorResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.TicketId);
            process.OnGetResourceFilesFromOperatorResult(message);

            return Task.CompletedTask;
        }
    }
}
