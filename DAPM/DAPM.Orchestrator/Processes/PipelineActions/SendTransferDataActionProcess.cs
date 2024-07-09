using DAPM.Orchestrator.Services;
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
    public class SendTransferDataActionProcess : OrchestratorProcess
    {

        private Guid _executionId;
        private Guid _stepId;

        private TransferDataActionDTO _data;
        private Guid _originOrganizationId;

        public SendTransferDataActionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, 
            Guid processId, TransferDataActionDTO data)
            : base(engine, serviceProvider, processId)
        {
            _data = data;
            _originOrganizationId = _data.OriginOrganizationId;

            _executionId = data.ExecutionId;
            _stepId = data.StepId;
        }

        public override void StartProcess()
        {
            var getOrganizationsProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOrganizationsMessage>>();


            var getOrganizationsMessage = new GetOrganizationsMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _originOrganizationId
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

            var sendTransferDataActionMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendTransferDataActionMessage>>();


            var transferDataActionMessage = new SendTransferDataActionMessage()
            {
                SenderProcessId = _processId,
                ExecutionId = _executionId,
                TimeToLive = TimeSpan.FromMinutes(1),
                SenderIdentity = identityDto,
                TargetPeerDomain = message.Organizations.First().Domain,
                Data = _data,
                StepId = _stepId,
            };

            sendTransferDataActionMessageProducer.PublishMessage(transferDataActionMessage);
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
                TimeToLive = TimeSpan.FromMinutes(1),
                ActionResult = actionResultDto
            };

            actionResultProducer.PublishMessage(actionResultMessage);

            EndProcess();
        }
    }
}
