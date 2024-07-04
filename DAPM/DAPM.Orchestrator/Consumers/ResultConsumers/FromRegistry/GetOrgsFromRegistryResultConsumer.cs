using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class GetOrgsFromRegistryResultConsumer : IQueueConsumer<GetOrganizationsResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetOrgsFromRegistryResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetOrganizationsResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnGetOrganizationsFromRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
