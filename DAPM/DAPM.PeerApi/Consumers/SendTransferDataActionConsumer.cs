using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendTransferDataActionConsumer : IQueueConsumer<SendTransferDataActionMessage>
    {
        private IHttpService _httpService;
        public SendTransferDataActionConsumer(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task ConsumeAsync(SendTransferDataActionMessage message)
        {
            var targetDomain = message.TargetPeerDomain;
            var senderIdentity = message.SenderIdentity;

            var transferDataActionDto = new TransferDataActionDto()
            {
                SenderIdentity = senderIdentity,
                Data = message.Data,
                StepId = message.StepId,
                ExecutionId = message.ExecutionId,
            };

            var url = "http://" + targetDomain + PeerApiEndpoints.TransferDataActionEndpoint;
            var body = JsonSerializer.Serialize(transferDataActionDto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}
