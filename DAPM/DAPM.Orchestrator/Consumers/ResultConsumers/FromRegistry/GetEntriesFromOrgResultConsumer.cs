using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class GetEntriesFromOrgResultConsumer : IQueueConsumer<GetEntriesFromOrgResult>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetEntriesFromOrgResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetEntriesFromOrgResult message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnGetEntriesFromOrgResult(message);

            return Task.CompletedTask;
        }
    }
}
