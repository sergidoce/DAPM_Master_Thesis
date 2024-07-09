using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class PostRepoToRegistryResultConsumer : IQueueConsumer<PostRepoToRegistryResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostRepoToRegistryResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostRepoToRegistryResultMessage message)
        {
            CreateRepositoryProcess process = (CreateRepositoryProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnPostRepoToRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
