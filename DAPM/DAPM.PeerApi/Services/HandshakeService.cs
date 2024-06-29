using DAPM.PeerApi.Models.HandshakeDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Services
{
    public class HandshakeService : IHandshakeService
    {
        private IQueueProducer<HandshakeRequestMessage> _handshakeRequestProducer;
        private IQueueProducer<HandshakeRequestResponseMessage> _handshakeRequestResponseProducer;


        public HandshakeService(IQueueProducer<HandshakeRequestMessage> handshakeRequestProducer,
            IQueueProducer<HandshakeRequestResponseMessage> handshakeRequestResponseProducer) 
        {
            _handshakeRequestProducer = handshakeRequestProducer;
            _handshakeRequestResponseProducer = handshakeRequestResponseProducer; 
        }
        

        public void OnHandshakeRequest(Guid handshakeId, IdentityDTO senderIdentity)
        {
            var message = new HandshakeRequestMessage()
            {
                TicketId = handshakeId,
                TimeToLive = TimeSpan.FromMinutes(1),
                SenderPeerIdentity = senderIdentity,
            };

            _handshakeRequestProducer.PublishMessage(message);
        }

        public void OnHandshakeRequestResponse(Guid handshakeId, IdentityDTO senderIdentity, bool IsAccepted)
        {
            var message = new HandshakeRequestResponseMessage()
            {
                TicketId = handshakeId,
                TimeToLive = TimeSpan.FromMinutes(1),
                SenderPeerIdentity = senderIdentity,
                IsRequestAccepted = IsAccepted
            };

            _handshakeRequestResponseProducer.PublishMessage(message);
        }
    }
}
