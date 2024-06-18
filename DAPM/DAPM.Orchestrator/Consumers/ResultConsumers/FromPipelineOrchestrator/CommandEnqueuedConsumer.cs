using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromPipelineOrchestrator
{
    public class CommandEnqueuedConsumer : IQueueConsumer<CommandEnqueuedMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public CommandEnqueuedConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(CommandEnqueuedMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.TicketId);
            process.OnCommandEnqueued(message);

            return Task.CompletedTask;
        }
    }
}
