using DAPM.PeerApi.Models;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class PostResourceFromPeerResultConsumer : IQueueConsumer<PostResourceFromPeerResultMessage>
    {
        private IHttpService _httpService;
        public PostResourceFromPeerResultConsumer(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ConsumeAsync(PostResourceFromPeerResultMessage message)
        {
            var targetDomain = message.TargetPeerDomain;

            var dto = new SendResourceToPeerResultDto()
            {
                StepId = message.TicketId,
                Succeeded = true,
            };

            var url = "http://" + targetDomain + PeerApiEndpoints.PostResourceResultEndpoint;
            var body = JsonSerializer.Serialize(dto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
