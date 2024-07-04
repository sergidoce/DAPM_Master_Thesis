using DAPM.PeerApi.Services.Interfaces;
using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.Other;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Services
{
    public class ActionService : IActionService
    {
        private IQueueProducer<ExecuteOperatorActionRequest> _executeOperatorRequestProducer;
        private IQueueProducer<TransferDataActionRequest> _transferDataRequestProducer;
        private IQueueProducer<ActionResultReceivedMessage> _actionResultReceivedMessageProducer;

        public ActionService(IQueueProducer<ExecuteOperatorActionRequest> executeOperatorRequestProducer,
            IQueueProducer<TransferDataActionRequest> transferDataRequestProducer,
            IQueueProducer<ActionResultReceivedMessage> actionResultReceivedMessageProducer)
        {
            _executeOperatorRequestProducer = executeOperatorRequestProducer;
            _transferDataRequestProducer = transferDataRequestProducer;
            _actionResultReceivedMessageProducer = actionResultReceivedMessageProducer;
        }

        public void OnActionResultReceived(Guid processId, ActionResultDTO actionResult)
        {

            var message = new ActionResultReceivedMessage()
            {
                ExecutionId = actionResult.ExecutionId,
                StepId = actionResult.StepId,
                ProcessId = processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Succeeded = true,
            };

            _actionResultReceivedMessageProducer.PublishMessage(message);
        }

        public void OnExecuteOperatorActionReceived(Guid senderProcessId, IdentityDTO senderIdentity, Guid stepId, ExecuteOperatorActionDTO data)
        {
            var message = new ExecuteOperatorActionRequest()
            {
                SenderProcessId = senderProcessId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrchestratorIdentity = senderIdentity,
                Data = data
            };

            _executeOperatorRequestProducer.PublishMessage(message);
        }

        public void OnTransferDataActionReceived(Guid senderProcessId, IdentityDTO senderIdentity, Guid stepId, TransferDataActionDTO data)
        {
            var message = new TransferDataActionRequest()
            {
                SenderProcessId = senderProcessId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrchestratorIdentity = senderIdentity,
                Data = data
            };

            _transferDataRequestProducer.PublishMessage(message);
        }
    }
}
