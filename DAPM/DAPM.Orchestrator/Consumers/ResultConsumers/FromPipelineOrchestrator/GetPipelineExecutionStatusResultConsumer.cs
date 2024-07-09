using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromPipelineOrchestrator
{
    public class GetPipelineExecutionStatusResultConsumer : IQueueConsumer<GetPipelineExecutionStatusResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetPipelineExecutionStatusResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetPipelineExecutionStatusResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnGetPipelineExecutionStatusResult(message);

            return Task.CompletedTask;
        }
    }
}
