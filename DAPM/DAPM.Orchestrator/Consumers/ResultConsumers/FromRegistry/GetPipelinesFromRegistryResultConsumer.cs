using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class GetPipelinesFromRegistryResultConsumer : IQueueConsumer<GetPipelinesResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetPipelinesFromRegistryResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetPipelinesResultMessage message)
        {
            GetPipelinesProcess process = (GetPipelinesProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnGetPipelinesFromRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
