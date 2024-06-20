using DAPM.PeerApi.Models.HandshakeDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi.Handshake;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendHandshakeAckConsumer : IQueueConsumer<SendHandshakeAckMessage>
    {
        private IHttpService _httpService;
        public SendHandshakeAckConsumer(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ConsumeAsync(SendHandshakeAckMessage message)
        {
            var targetDomain = message.TargetPeerDomain;
            var senderIdentity = message.SenderPeerIdentity;


            var handshakeAckDto = new HandshakeAckDto()
            {
                HandshakeId = message.TicketId,
                IsDone = message.HandshakeAck.IsCompleted,
                SenderIdentity = senderIdentity,
            };

            var url = "http://" + targetDomain + PeerApiEndpoints.HandshakeAckEndpoint;
            var body = JsonSerializer.Serialize(handshakeAckDto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
