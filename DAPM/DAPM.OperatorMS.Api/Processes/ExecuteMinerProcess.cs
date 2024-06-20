
using DAPM.OperatorMS.Api.Services;
using RabbitMQLibrary.Messages.Operator;
using DAPM.OperatorMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.OperatorMS.Api.Processes
{
    public class ExecuteMinerProcess : OperatorProcess
    {
        private ExecuteMinerMessage _message;
        private IOperatorService _operatorService;

        public ExecuteMinerProcess(OperatorEngine engine, IServiceProvider serviceProvider, Guid ticketId, ExecuteMinerMessage message) : base(engine, serviceProvider, ticketId)
        {
            _message = message;
        }

        public override async void StartProcess()
        {
            FileDTO executionResult = await _operatorService.ExecuteMiner(_message.ExecutionId, _message.StepId);
            executionResult.Name = _message.OutputName;

            IEnumerable<FileDTO> files = new List<FileDTO>();
            files.Append(executionResult);

            var executeMinerResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostResourceRequest>>();

            var executeMinerResultMessage = new PostResourceRequest()
            {
                TicketId = _ticketId,
                OrganizationId = _message.OrganizationId,
                RepositoryId = _message.RepositoryId,
                Name = $"output_{_message.ExecutionId}_{_message.StepId}",
                ResourceType = "Execution Output",
                TimeToLive = TimeSpan.FromMinutes(1),
                Files = files,
            };

            executeMinerResultProducer.PublishMessage(executeMinerResultMessage);

            EndProcess();
        }
    }
}
