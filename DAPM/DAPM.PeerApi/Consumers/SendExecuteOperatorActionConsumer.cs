﻿using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using System.Text.Json;

namespace DAPM.PeerApi.Consumers
{
    public class SendExecuteOperatorActionConsumer : IQueueConsumer<SendExecuteOperatorActionMessage>
    {
        private IHttpService _httpService;
        public SendExecuteOperatorActionConsumer(IHttpService httpService)
        {
            _httpService = httpService;
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

            var url = "http://" + targetDomain + PeerApiEndpoints.ExecuteOperatorActionEndpoint;
            var body = JsonSerializer.Serialize(transferDataActionDto);

            var response = await _httpService.SendPostRequestAsync(url, body);

            return;
        }
    }
}