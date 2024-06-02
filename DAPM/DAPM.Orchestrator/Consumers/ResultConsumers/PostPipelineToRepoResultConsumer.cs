using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;

namespace DAPM.Orchestrator.Consumers.ResultConsumers
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
            PostPipelineProcess process = (PostPipelineProcess)_orchestratorEngine.GetProcess(message.TicketId);
            process.OnPostPipelineToRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
