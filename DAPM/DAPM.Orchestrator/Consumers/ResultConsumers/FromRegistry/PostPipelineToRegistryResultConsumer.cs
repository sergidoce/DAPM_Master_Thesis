using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class PostPipelineToRegistryResultConsumer : IQueueConsumer<PostPipelineToRegistryResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostPipelineToRegistryResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostPipelineToRegistryResultMessage message)
        {
            PostPipelineProcess process = (PostPipelineProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnPostPipelineToRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
