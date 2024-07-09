using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Messages.Repository;

namespace DAPM.Orchestrator.Processes.PipelineCommands
{
    public class GetPipelineExecutionStatusProcess : OrchestratorProcess
    {

        private Guid _executionId;
        private Guid _ticketId;

        public GetPipelineExecutionStatusProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid processId, Guid executionId) : base(engine, serviceProvider, processId)
        {
            _ticketId = ticketId;
            _executionId = executionId;
        }

        public override void StartProcess()
        {
            var getPipelineExecutionStatusProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelineExecutionStatusMessage>>();

            var message = new GetPipelineExecutionStatusMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ExecutionId = _executionId,
                
            };

            getPipelineExecutionStatusProducer.PublishMessage(message);
        }


        public override void OnGetPipelineExecutionStatusResult(GetPipelineExecutionStatusResultMessage message)
        {
            var getPipelineExecutionStatusResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelineExecutionStatusRequestResult>>();

            var resultMessage = new GetPipelineExecutionStatusRequestResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Status = message.Status,

            };

            getPipelineExecutionStatusResultProducer.PublishMessage(resultMessage);
        }
    }
}
