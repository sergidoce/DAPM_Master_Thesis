using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PipelineOrchestrator;

namespace DAPM.PipelineOrchestratorMS.Api.Consumers
{
    public class CreateInstanceExecutionConsumer : IQueueConsumer<CreateInstanceExecutionMessage>
    {

        private ILogger<CreateInstanceExecutionConsumer> _logger;
        private IPipelineOrchestrationEngine _pipelineOrchestrationEngine;

        public CreateInstanceExecutionConsumer(ILogger<CreateInstanceExecutionConsumer> logger, IPipelineOrchestrationEngine pipelineOrchestrationEngine)
        {
            _logger = logger;
            _pipelineOrchestrationEngine = pipelineOrchestrationEngine;
        }
        public Task ConsumeAsync(CreateInstanceExecutionMessage message)
        {
            _logger.LogInformation("CreateInstanceExecutionMessage received");


            _pipelineOrchestrationEngine.CreateNewExecutionInstance(message.Pipeline);

            return Task.CompletedTask;
        }
    }
}
