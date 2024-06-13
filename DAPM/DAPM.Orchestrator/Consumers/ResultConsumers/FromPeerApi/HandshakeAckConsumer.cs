using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromPeerApi
{
    public class HandshakeAckConsumer : IQueueConsumer<HandshakeAckMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public HandshakeAckConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(HandshakeAckMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.TicketId);
            process.OnHandshakeAck(message);

            return Task.CompletedTask;
        }
    }
}
