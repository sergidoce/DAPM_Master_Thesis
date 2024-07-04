using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendActionResultConsumer : IQueueConsumer<SendActionResultMessage>
    {
        private IHttpService _httpService;
        public SendActionResultConsumer(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ConsumeAsync(SendActionResultMessage message)
        {
            var targetDomain = message.TargetPeerDomain;

            var actionResultDto = new ActionResultDto()
            {
                ProcessId = message.SenderProcessId,
                StepId = message.ActionResult.StepId,
                ExecutionId = message.ActionResult.ExecutionId,
                ActionResult = (int)message.ActionResult.ActionResult,
                Message = "Action completed successfully"
            };

            var url = "http://" + targetDomain + PeerApiEndpoints.ActionResultEndpoint;
            var body = JsonSerializer.Serialize(actionResultDto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
