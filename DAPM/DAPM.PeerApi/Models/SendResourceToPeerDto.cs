using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models
{
    public class SendResourceToPeerDto
    {
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public IdentityDTO SenderPeerIdentity { get; set; }
        public int StorageMode { get; set; }
        public Guid? RepositoryId { get; set; }
        public ResourceDTO Resource { get; set; }
    }
}
