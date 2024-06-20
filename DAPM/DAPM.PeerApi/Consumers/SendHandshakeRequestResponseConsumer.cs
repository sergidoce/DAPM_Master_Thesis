using DAPM.PeerApi.Models.HandshakeDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi.Handshake;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendHandshakeRequestResponseConsumer : IQueueConsumer<SendHandshakeRequestResponseMessage>
    {
        private IHttpService _httpService;
        public SendHandshakeRequestResponseConsumer(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ConsumeAsync(SendHandshakeRequestResponseMessage message)
        {
            var targetDomain = message.TargetDomain;
            var senderIdentity = message.SenderPeerIdentity;

            var handshakeRequestResponseDto = new HandshakeRequestResponseDto()
            {
                SenderIdentity = senderIdentity,
                HandshakeId = message.TicketId,
                IsAccepted = message.IsRequestAccepted,
            };

            var url = "http://" + targetDomain + PeerApiEndpoints.HandshakeRequestResponseEndpoint;
            var body = JsonSerializer.Serialize(handshakeRequestResponseDto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
