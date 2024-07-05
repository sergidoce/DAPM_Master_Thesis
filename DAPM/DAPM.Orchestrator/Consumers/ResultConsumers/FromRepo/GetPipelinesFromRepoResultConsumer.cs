using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
{
    public class GetPipelinesFromRepoResultConsumer : IQueueConsumer<GetPipelinesFromRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetPipelinesFromRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetPipelinesFromRepoResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnGetPipelinesFromRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
