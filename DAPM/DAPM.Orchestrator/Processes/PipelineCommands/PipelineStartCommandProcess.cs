using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.PeerApi.Handshake;
using RabbitMQLibrary.Messages.PipelineOrchestrator;

namespace DAPM.Orchestrator.Processes.PipelineCommands
{
    public class PipelineStartCommandProcess : OrchestratorProcess
    {
        private Guid _executionId;
        private Guid _ticketId;

        public PipelineStartCommandProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid processId, Guid executionId)
            : base(engine, serviceProvider, processId)
        {
            _executionId = executionId;
            _ticketId = ticketId;
        }

        public override void StartProcess()
        {
            var pipelineStartCommandProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PipelineStartCommand>>();

            var commandMessage = new PipelineStartCommand()
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = _ticketId,
                ExecutionId = _executionId,
            };

            pipelineStartCommandProducer.PublishMessage(commandMessage);
        }

        public override void OnCommandEnqueued(CommandEnqueuedMessage message)
        {

            var postPipelineCommandProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostPipelineCommandProcessResult>>();

            var postPipelineCommandProcessResultMessage = new PostPipelineCommandProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Message = "The command was successfully enqueued",
                Succeeded = true,
                ExecutionId = _executionId
            };

            postPipelineCommandProcessResultProducer.PublishMessage(postPipelineCommandProcessResultMessage);

            EndProcess();
        }

    }
}
