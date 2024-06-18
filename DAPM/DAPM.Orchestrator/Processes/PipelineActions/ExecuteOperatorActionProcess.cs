using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes.PipelineActions
{
    public class ExecuteOperatorActionProcess : OrchestratorProcess
    {
        private Guid _stepId;
        private Guid _executionId;

        public ExecuteOperatorActionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, ExecuteOperatorActionDTO data) 
            : base(engine, serviceProvider, ticketId)
        {
            _stepId = data.StepId;
            _executionId = data.ExecutionId;
        }

        public override void StartProcess()
        {
            var actionResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ActionResultMessage>>();


            var actionResultDto = new ActionResultDTO()
            {
                ActionResult = ActionResult.Completed,
                ExecutionId = _executionId,
                StepId = _stepId,
                Message = "Step completed"
            };


            var message = new ActionResultMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ActionResult = actionResultDto
            };

            actionResultProducer.PublishMessage(message);

            EndProcess();
        }
    }
}
