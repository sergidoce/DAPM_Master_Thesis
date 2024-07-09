using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class GetReposFromRegistryResultConsumer : IQueueConsumer<GetRepositoriesResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetReposFromRegistryResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetRepositoriesResultMessage message)
        {
            GetRepositoriesProcess process = (GetRepositoriesProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnGetRepositoriesFromRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
