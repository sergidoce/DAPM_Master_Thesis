using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromPeerApi
{
    public class RegistryUpdateAckConsumer : IQueueConsumer<RegistryUpdateAckMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public RegistryUpdateAckConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(RegistryUpdateAckMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnRegistryUpdateAck(message);

            return Task.CompletedTask;
        }
    }
}
