using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
{
    public class GetOperatorFilesFromRepoResultConsumer : IQueueConsumer<GetOperatorFilesFromRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetOperatorFilesFromRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetOperatorFilesFromRepoResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnGetOperatorFilesFromRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
