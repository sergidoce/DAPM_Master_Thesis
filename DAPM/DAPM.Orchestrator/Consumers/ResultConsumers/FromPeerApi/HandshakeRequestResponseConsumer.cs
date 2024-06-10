using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromPeerApi
{
    public class HandshakeRequestResponseConsumer : IQueueConsumer<HandshakeRequestResponseMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public HandshakeRequestResponseConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(HandshakeRequestResponseMessage message)
        {
            CollabHandshakeProcess process = (CollabHandshakeProcess)_orchestratorEngine.GetProcess(message.TicketId);
            process.OnHandshakeRequestResponse(message);

            return Task.CompletedTask;
        }
    }
}
