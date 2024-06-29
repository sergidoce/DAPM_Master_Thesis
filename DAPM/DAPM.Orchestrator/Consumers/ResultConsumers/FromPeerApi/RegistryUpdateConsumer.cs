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
            var isPartOfHandshake = message.IsPartOfHandshake;

            if (isPartOfHandshake)
            {
                OrchestratorProcess process = _orchestratorEngine.GetProcess(message.TicketId);
                process.OnRegistryUpdate(message);
            }
            else
            {
                _orchestratorEngine.StartRegistryUpdateProcess(message.TicketId, message.RegistryUpdate, message.SenderIdentity);
            }
            
            return Task.CompletedTask;
        }
    }
}
