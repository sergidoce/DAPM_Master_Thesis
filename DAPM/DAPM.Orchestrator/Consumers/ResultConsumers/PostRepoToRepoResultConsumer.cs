﻿using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;

namespace DAPM.Orchestrator.Consumers.ResultConsumers
{
    public class PostRepoToRepoResultConsumer : IQueueConsumer<PostRepoToRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostRepoToRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostRepoToRepoResultMessage message)
        {
            CreateRepositoryProcess process = (CreateRepositoryProcess)_orchestratorEngine.GetProcess(message.TicketId);
            process.OnCreateRepoInRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
