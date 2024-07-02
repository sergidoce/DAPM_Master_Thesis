using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendExecuteOperatorActionConsumer : IQueueConsumer<SendExecuteOperatorActionMessage>
    {
        ILogger<SendExecuteOperatorActionConsumer> _logger;
        private IHttpService _httpService;
        public SendExecuteOperatorActionConsumer(IHttpService httpService, ILogger<SendExecuteOperatorActionConsumer> logger)
        {
            _httpService = httpService;
            _logger = logger;
        }

        public async Task ConsumeAsync(SendExecuteOperatorActionMessage message)
        {
            var targetDomain = message.TargetPeerDomain;
            var senderIdentity = message.SenderIdentity;

            var transferDataActionDto = new ExecuteOperatorActionDto()
            {
                SenderIdentity = senderIdentity,
                Data = message.Data,
                StepId = message.StepId,
                ExecutionId = message.ExecutionId,
            };

            _logger.LogInformation(transferDataActionDto.ToString());

            var url = "http://" + targetDomain + PeerApiEndpoints.ExecuteOperatorActionEndpoint;
            var body = JsonSerializer.Serialize(transferDataActionDto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
