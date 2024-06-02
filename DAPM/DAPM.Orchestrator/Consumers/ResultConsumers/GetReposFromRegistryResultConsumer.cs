using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;

namespace DAPM.Orchestrator.Consumers.ResultConsumers
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
            GetRepositoriesProcess process = (GetRepositoriesProcess)_orchestratorEngine.GetProcess(message.TicketId);
            process.OnGetRepositoriesFromRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
