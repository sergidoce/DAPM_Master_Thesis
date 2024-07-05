using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
{
    public class PostRepoToRepoResultConsumer : IQueueConsumer<PostRepoToRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostRepoToRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostRepoToRepoResultMessage message)
        {
            CreateRepositoryProcess process = (CreateRepositoryProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnCreateRepoInRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
