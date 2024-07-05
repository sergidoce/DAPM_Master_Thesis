using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.PipelineOrchestrator;

namespace DAPM.PipelineOrchestratorMS.Api.Consumers
{
    public class CreateInstanceExecutionConsumer : IQueueConsumer<CreateInstanceExecutionMessage>
    {

        private ILogger<CreateInstanceExecutionConsumer> _logger;
        private IPipelineOrchestrationEngine _pipelineOrchestrationEngine;
        private IQueueProducer<CreatePipelineExecutionResultMessage> _createExecutionResultProducer;

        public CreateInstanceExecutionConsumer(ILogger<CreateInstanceExecutionConsumer> logger, 
            IPipelineOrchestrationEngine pipelineOrchestrationEngine,
            IQueueProducer<CreatePipelineExecutionResultMessage> createExecutionResultProducer)
        {
            _logger = logger;
            _pipelineOrchestrationEngine = pipelineOrchestrationEngine;
            _createExecutionResultProducer = createExecutionResultProducer;
        }
        public Task ConsumeAsync(CreateInstanceExecutionMessage message)
        {
            _logger.LogInformation("CreateInstanceExecutionMessage received");

            var executionId = _pipelineOrchestrationEngine.CreatePipelineExecutionInstance(message.Pipeline.Pipeline);

            var resultMessage = new CreatePipelineExecutionResultMessage()
            {
                ProcessId = message.ProcessId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Succeeded = true,
                PipelineExecutionId = executionId,
            };

            _createExecutionResultProducer.PublishMessage(resultMessage);

            return Task.CompletedTask;
        }
    }
}
