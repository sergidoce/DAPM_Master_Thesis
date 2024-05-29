using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;

namespace DAPM.Orchestrator.Consumers.ResultConsumers
{
    public class PostResourceToRepoResultConsumer : IQueueConsumer<PostResourceToRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostResourceToRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostResourceToRepoResultMessage message)
        {
            PostResourceProcess process = (PostResourceProcess)_orchestratorEngine.GetProcess(message.TicketId);
            process.OnPostResourceToRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
