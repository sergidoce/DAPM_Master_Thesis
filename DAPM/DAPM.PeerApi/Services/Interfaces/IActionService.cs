using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Services.Interfaces
{
    public interface IActionService
    {
        public void OnTransferDataActionReceived(Guid senderProcessId, IdentityDTO senderIdentity, Guid stepId, TransferDataActionDTO data);
        public void OnExecuteOperatorActionReceived(Guid senderProcessId, IdentityDTO senderIdentity, Guid stepId, ExecuteOperatorActionDTO data);
        public void OnActionResultReceived(ActionResultDTO actionResult);

    }
}
