using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Consumers
{
    public class GetPipelineExecutionStatusConsumer : IQueueConsumer<GetPipelineExecutionStatusMessage>
    {
        private IPipelineOrchestrationEngine _pipelineOrchestrationEngine;
        private IQueueProducer<GetPipelineExecutionStatusResultMessage> _getPipelineExecutionStatusResultProducer;

        public GetPipelineExecutionStatusConsumer(IPipelineOrchestrationEngine pipelineOrchestrationEngine,
            IQueueProducer<GetPipelineExecutionStatusResultMessage> getPipelineExecutionStatusResultProducer)
        {
            _pipelineOrchestrationEngine = pipelineOrchestrationEngine;
            _getPipelineExecutionStatusResultProducer = getPipelineExecutionStatusResultProducer;
        }

        public Task ConsumeAsync(GetPipelineExecutionStatusMessage message)
        {
            var status = _pipelineOrchestrationEngine.GetPipelineExecutionStatus(message.ExecutionId);


            var currentStepsDtos = new List<StepStatusDTO>();
            foreach(var step in status.CurrentSteps)
            {
                var stepDto = new StepStatusDTO()
                {
                    StepId = step.StepId,
                    ExecutionerPeer = step.ExecutionerPeer,
                    ExecutionTime = step.ExecutionTime,
                    StepType = step.StepType,
                };
                currentStepsDtos.Add(stepDto);
            }


            var pipelineExecutionStatusDto = new PipelineExecutionStatusDTO()
            {
                ExecutionTime = status.ExecutionTime,
                State = status.State.ToString(),
                CurrentSteps = currentStepsDtos,
            };

            var getPipelineExecutionStatusResultMessage = new GetPipelineExecutionStatusResultMessage()
            {
                ProcessId = message.ProcessId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Status = pipelineExecutionStatusDto
            };

            _getPipelineExecutionStatusResultProducer.PublishMessage(getPipelineExecutionStatusResultMessage);

            return Task.CompletedTask;
        }
    }
}
