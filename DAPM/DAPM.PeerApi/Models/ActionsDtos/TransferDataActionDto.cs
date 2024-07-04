using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models.ActionsDtos
{
    public class TransferDataActionDto
    {
        public Guid SenderProcessId { get; set; }
        public IdentityDTO SenderIdentity { get; set; }
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public TransferDataActionDTO Data { get; set; }
    }
}
