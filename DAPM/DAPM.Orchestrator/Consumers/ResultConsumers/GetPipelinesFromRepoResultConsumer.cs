using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;

namespace DAPM.Orchestrator.Consumers.ResultConsumers
{
    public class GetPipelinesFromRepoResultConsumer : IQueueConsumer<GetPipelinesFromRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetPipelinesFromRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetPipelinesFromRepoResultMessage message)
        {
            GetPipelinesProcess process = (GetPipelinesProcess)_orchestratorEngine.GetProcess(message.TicketId);
            process.OnGetPipelinesFromRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
