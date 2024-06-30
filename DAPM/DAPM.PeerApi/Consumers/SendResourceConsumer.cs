using DAPM.PeerApi.Models;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendResourceConsumer : IQueueConsumer<SendResourceToPeerMessage>
    {
        private IHttpService _httpService;
        public SendResourceConsumer(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ConsumeAsync(SendResourceToPeerMessage message)
        {
            var targetDomain = message.TargetPeerDomain;
            var senderIdentity = message.SenderPeerIdentity;

            var dto = new SendResourceToPeerDto()
            {
                StepId = message.TicketId,
                ExecutionId = message.ExecutionId,
                SenderPeerIdentity = senderIdentity,
                StorageMode = message.StorageMode,
                RepositoryId = message.RepositoryId,
                Resource = message.Resource,
            };

            var url = "http://" + targetDomain + PeerApiEndpoints.PostResourceEndpoint;
            var body = JsonSerializer.Serialize(dto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
