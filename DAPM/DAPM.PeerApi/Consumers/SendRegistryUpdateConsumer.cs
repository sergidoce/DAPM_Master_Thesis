using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Models.HandshakeDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendRegistryUpdateConsumer : IQueueConsumer<SendRegistryUpdateMessage>
    {
        private IHttpService _httpService;
        public SendRegistryUpdateConsumer(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ConsumeAsync(SendRegistryUpdateMessage message)
        {
            var targetDomain = message.TargetPeerDomain;
            var senderIdentity = message.SenderPeerIdentity;

            var registryUpdateDto = new RegistryUpdateDto()
            {
                SenderIdentity = senderIdentity,
                RegistryUpdateId = message.TicketId,
                IsPartOfHandshake = message.IsPartOfHandshake,
                Organizations = message.RegistryUpdate.Organizations,
                Repositories = message.RegistryUpdate.Repositories,
                Resources = message.RegistryUpdate.Resources,
                Pipelines = message.RegistryUpdate.Pipelines,
            };

            var url = "http://" + targetDomain + PeerApiEndpoints.RegistryUpdateEndpoint;
            var body = JsonSerializer.Serialize(registryUpdateDto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
