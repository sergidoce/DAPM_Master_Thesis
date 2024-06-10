using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromPeerApi
{
    public class RegistryUpdateConsumer : IQueueConsumer<RegistryUpdateMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public RegistryUpdateConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(RegistryUpdateMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.TicketId);
            process.OnRegistryUpdate(message);

            return Task.CompletedTask;
        }
    }
}
