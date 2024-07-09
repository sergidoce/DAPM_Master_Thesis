using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
{
    public class PostPipelineToRepoResultConsumer : IQueueConsumer<PostPipelineToRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostPipelineToRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostPipelineToRepoResultMessage message)
        {
            PostPipelineProcess process = (PostPipelineProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnPostPipelineToRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
