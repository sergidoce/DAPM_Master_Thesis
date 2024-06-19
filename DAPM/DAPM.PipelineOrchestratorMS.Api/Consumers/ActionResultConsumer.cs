using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PipelineOrchestrator;

namespace DAPM.PipelineOrchestratorMS.Api.Consumers
{
    public class ActionResultConsumer : IQueueConsumer<ActionResultMessage>
    {

        private IPipelineOrchestrationEngine _pipelineOrchestrationEngine;

        public ActionResultConsumer(IPipelineOrchestrationEngine pipelineOrchestrationEngine)
        {
            _pipelineOrchestrationEngine = pipelineOrchestrationEngine;
        }

        public Task ConsumeAsync(ActionResultMessage message)
        {
            _pipelineOrchestrationEngine.ProcessActionResult(message.ActionResult);
            return Task.CompletedTask;
        }
    }
}
