using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi;
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

            var url = "http://" + targetDomain + PeerApiEndpoints.HandshakeAckEndpoint;
            var body = JsonSerializer.Serialize(message.HandshakeAck);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
