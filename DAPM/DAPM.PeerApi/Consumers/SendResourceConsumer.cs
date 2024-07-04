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
        private ILogger<SendResourceConsumer> _logger;
        public SendResourceConsumer(IHttpService httpService, ILogger<SendResourceConsumer> logger)
        {
            _httpService = httpService;
            _logger = logger;
        }

        public async Task ConsumeAsync(SendResourceToPeerMessage message)
        {
            var targetDomain = message.TargetPeerDomain;
            var senderIdentity = message.SenderPeerIdentity;

            _logger.LogInformation("Ticket id / step id in SendResourceConsumer is " + message.SenderProcessId.ToString());

            var dto = new SendResourceToPeerDto()
            {
                StepId = message.SenderProcessId,
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
