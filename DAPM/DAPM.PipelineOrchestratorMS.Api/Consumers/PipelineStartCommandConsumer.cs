using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.PipelineOrchestrator;

namespace DAPM.PipelineOrchestratorMS.Api.Consumers
{
    public class PipelineStartCommandConsumer : IQueueConsumer<PipelineStartCommand>
    {
        private IPipelineOrchestrationEngine _pipelineOrchestrationEngine;
        private IQueueProducer<CommandEnqueuedMessage> _commandEnqueuedProducer;

        public PipelineStartCommandConsumer(IPipelineOrchestrationEngine pipelineOrchestrationEngine,
            IQueueProducer<CommandEnqueuedMessage> commandEnqueuedProducer)
        {
            _pipelineOrchestrationEngine = pipelineOrchestrationEngine;
            _commandEnqueuedProducer = commandEnqueuedProducer;
        }

        public Task ConsumeAsync(PipelineStartCommand message)
        {
            _pipelineOrchestrationEngine.ExecutePipelineStartCommand(message.ExecutionId);

            var commandEnqueuedMessage = new CommandEnqueuedMessage()
            {
                TicketId = message.TicketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Succeeded = true,
                Command = "start",
                Message = "Command enqueued successfully"
            };

            _commandEnqueuedProducer.PublishMessage(commandEnqueuedMessage);

            return Task.CompletedTask;
        }
    }
}
