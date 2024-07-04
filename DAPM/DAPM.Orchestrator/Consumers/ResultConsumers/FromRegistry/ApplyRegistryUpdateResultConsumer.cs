using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class ApplyRegistryUpdateResultConsumer : IQueueConsumer<ApplyRegistryUpdateResult>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public ApplyRegistryUpdateResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(ApplyRegistryUpdateResult message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnApplyRegistryUpdateResult(message);

            return Task.CompletedTask;
        }
    }
}
