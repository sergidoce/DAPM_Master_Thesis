﻿using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
{
    public class GetResourceFilesFromRepoResultConsumer : IQueueConsumer<GetResourceFilesFromRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetResourceFilesFromRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetResourceFilesFromRepoResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnGetResourceFilesFromRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
