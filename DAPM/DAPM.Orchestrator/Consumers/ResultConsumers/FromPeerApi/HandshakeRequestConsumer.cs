using DAPM.Orchestrator.Services.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromPeerApi
{
    public class HandshakeRequestConsumer : IQueueConsumer<HandshakeRequestMessage>
    {
        private IOrchestratorEngine _engine;

        public HandshakeRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }
        public Task ConsumeAsync(HandshakeRequestMessage message)
        {
            var senderIdentity = new Identity()
            {
                Id = message.SenderPeerIdentity.Id,
                Name = message.SenderPeerIdentity.Name,
                Domain = message.SenderPeerIdentity.Domain,
            };

            _engine.StartCollabHandshakeResponseProcess(message.TicketId, senderIdentity);
            return Task.CompletedTask;
        }
    }
}
