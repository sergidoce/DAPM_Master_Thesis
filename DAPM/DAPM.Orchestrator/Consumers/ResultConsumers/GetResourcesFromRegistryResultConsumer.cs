using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;

namespace DAPM.Orchestrator.Consumers.ResultConsumers
{
    public class GetResourcesFromRegistryResultConsumer : IQueueConsumer<GetResourcesResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetResourcesFromRegistryResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetResourcesResultMessage message)
        {
            GetResourcesProcess process = (GetResourcesProcess)_orchestratorEngine.GetProcess(message.TicketId);
            process.OnGetResourcesFromRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
