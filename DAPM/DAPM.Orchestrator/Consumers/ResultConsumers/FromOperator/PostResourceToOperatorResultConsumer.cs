using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromOperator
{
    public class PostResourceToOperatorResultConsumer : IQueueConsumer<PostInputResourceResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostResourceToOperatorResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostInputResourceResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.TicketId);
            process.OnPostResourceToOperatorResult(message);

            return Task.CompletedTask;
        }
    }
}
