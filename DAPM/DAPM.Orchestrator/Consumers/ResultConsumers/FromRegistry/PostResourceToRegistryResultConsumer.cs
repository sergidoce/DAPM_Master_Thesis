using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class PostResourceToRegistryResultConsumer : IQueueConsumer<PostResourceToRegistryResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostResourceToRegistryResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostResourceToRegistryResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnPostResourceToRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
