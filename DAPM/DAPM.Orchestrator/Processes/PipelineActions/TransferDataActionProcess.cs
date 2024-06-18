
using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes.PipelineActions
{
    public class TransferDataActionProcess : OrchestratorProcess
    {
        private Guid _executionId;
        private Guid _stepId;

        public TransferDataActionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, TransferDataActionDTO data) 
            : base(engine, serviceProvider, ticketId)
        {
            _executionId = data.ExecutionId;
            _stepId = data.StepId;
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
