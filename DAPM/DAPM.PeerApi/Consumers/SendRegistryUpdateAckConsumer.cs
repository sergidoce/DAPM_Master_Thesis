using DAPM.PeerApi.Models;
using DAPM.PeerApi.Models.HandshakeDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendRegistryUpdateAckConsumer : IQueueConsumer<SendRegistryUpdateAckMessage>
    {
        private IHttpService _httpService;
        public SendRegistryUpdateAckConsumer(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ConsumeAsync(SendRegistryUpdateAckMessage message)
        {
            var targetDomain = message.TargetPeerDomain;
            var senderIdentity = message.SenderPeerIdentity;


            var registryUpdateAckDto = new RegistryUpdateAckDto()
            {
                RegistryUpdateId = message.TicketId,
                IsDone = message.RegistryUpdateAck.IsCompleted,
                SenderIdentity = senderIdentity,
            };

            var url = "http://" + targetDomain + PeerApiEndpoints.RegistryUpdateAckEndpoint;
            var body = JsonSerializer.Serialize(registryUpdateAckDto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
