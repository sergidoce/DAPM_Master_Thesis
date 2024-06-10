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
            CollabHandshakeProcess process = (CollabHandshakeProcess)_orchestratorEngine.GetProcess(message.TicketId);
            process.OnGetEntriesFromOrgResult(message);

            return Task.CompletedTask;
        }
    }
}
