using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromPipelineOrchestrator
{
    public class CreatePipelineExecutionResultConsumer : IQueueConsumer<CreatePipelineExecutionResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public CreatePipelineExecutionResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(CreatePipelineExecutionResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.TicketId);
            process.OnCreatePipelineExecutionResult(message);

            return Task.CompletedTask;
        }
    }
}
