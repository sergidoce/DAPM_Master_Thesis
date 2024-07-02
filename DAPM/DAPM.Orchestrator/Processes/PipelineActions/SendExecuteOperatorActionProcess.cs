using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.Other;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes.PipelineActions
{
    public class SendExecuteOperatorActionProcess : OrchestratorProcess
    {
     
        private Guid _executionId;
        private Guid _stepId;

        private ExecuteOperatorActionDTO _data;
        private Guid _destinationOrganizationId;

        public SendExecuteOperatorActionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, ExecuteOperatorActionDTO data)
            : base(engine, serviceProvider, ticketId)
        {
            _data = data;
            _destinationOrganizationId = _data.OperatorResource.OrganizationId;

            _executionId = data.ExecutionId;
            _stepId = data.StepId;
        }


        public override void StartProcess()
        {
            var getOrganizationsProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOrganizationsMessage>>();


            var getOrganizationsMessage = new GetOrganizationsMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _destinationOrganizationId
            };

            getOrganizationsProducer.PublishMessage(getOrganizationsMessage);
        }

        public override void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {
            var identityDto = new IdentityDTO()
            {
                Id = _localPeerIdentity.Id,
                Name = _localPeerIdentity.Name,
                Domain = _localPeerIdentity.Domain
            };

            var sendExecuteOperatorActionMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendExecuteOperatorActionMessage>>();


            var executeOperatorActionMessage = new SendExecuteOperatorActionMessage()
            {
                TicketId = _ticketId,
                ExecutionId = _executionId,
                TimeToLive = TimeSpan.FromMinutes(1),
                SenderIdentity = identityDto,
                TargetPeerDomain = message.Organizations.First().Domain,
                Data = _data,
                StepId = _stepId,
            };

            sendExecuteOperatorActionMessageProducer.PublishMessage(executeOperatorActionMessage);
        }

        public override void OnActionResultFromPeer(ActionResultReceivedMessage message)
        {
            var actionResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ActionResultMessage>>();

            var actionResultDto = new ActionResultDTO()
            {
                ActionResult = ActionResult.Completed,
                ExecutionId = _executionId,
                StepId = _stepId,
                Message = "Step completed"
            };


            var actionResultMessage = new ActionResultMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ActionResult = actionResultDto
            };

            actionResultProducer.PublishMessage(actionResultMessage);

            EndProcess();
        }

    }
}
