using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
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
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnPostResourceToRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
