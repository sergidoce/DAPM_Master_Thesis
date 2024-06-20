using DAPM.PeerApi.Models.HandshakeDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Services
{
    public class HandshakeService : IHandshakeService
    {
        private IQueueProducer<HandshakeAckMessage> _handshakeAckProducer;
        private IQueueProducer<HandshakeRequestMessage> _handshakeRequestProducer;
        private IQueueProducer<HandshakeRequestResponseMessage> _handshakeRequestResponseProducer;


        public HandshakeService(IQueueProducer<HandshakeAckMessage> handshakeAckProducer, 
            IQueueProducer<HandshakeRequestMessage> handshakeRequestProducer,
            IQueueProducer<HandshakeRequestResponseMessage> handshakeRequestResponseProducer) 
        {
            _handshakeAckProducer = handshakeAckProducer;
            _handshakeRequestProducer = handshakeRequestProducer;
            _handshakeRequestResponseProducer = handshakeRequestResponseProducer; 
        }
        public void OnHandshakeAck(Guid handshakeId, IdentityDTO senderIdentity, HandshakeAckDto handshakeAck)
        {
            var rabbitMQDto = new HandshakeAckDTO()
            {
                IsCompleted = handshakeAck.IsDone
            };

            var message = new HandshakeAckMessage()
            {
                TicketId = handshakeId,
                TimeToLive = TimeSpan.FromMinutes(1),
                PeerSenderIdentity = senderIdentity,
                HandshakeAck = rabbitMQDto
            };

            _handshakeAckProducer.PublishMessage(message);
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
