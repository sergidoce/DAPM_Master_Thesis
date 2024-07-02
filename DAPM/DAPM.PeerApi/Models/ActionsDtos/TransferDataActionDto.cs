using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models.ActionsDtos
{
    public class TransferDataActionDto
    {
        public IdentityDTO SenderIdentity { get; set; }
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public TransferDataActionDTO Data { get; set; }
    }
}
