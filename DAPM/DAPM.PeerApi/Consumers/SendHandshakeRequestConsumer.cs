using DAPM.PeerApi.Models.HandshakeDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendHandshakeRequestConsumer : IQueueConsumer<SendHandshakeRequestMessage>
    {
        private IHttpService _httpService;
        public SendHandshakeRequestConsumer(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ConsumeAsync(SendHandshakeRequestMessage message)
        {
            var targetDomain = message.RequestedPeerDomain;
            var senderIdentity = message.SenderPeerIdentity;

            var handshakeRequestDto = new HandshakeRequestDto()
            {
                SenderIdentity = senderIdentity,
                HandshakeId = message.TicketId,
            };

            var url = "http://" + targetDomain + PeerApiEndpoints.HandshakeRequestEndpoint;
            var body = JsonSerializer.Serialize(handshakeRequestDto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return; 
        }
    }
}
