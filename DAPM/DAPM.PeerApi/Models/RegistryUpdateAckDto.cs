using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models
{
    public class RegistryUpdateAckDto
    {
        public IdentityDTO SenderIdentity { get; set; }
        public bool IsPartOfHandshake { get; set; }
        public Guid RegistryUpdateId { get; set; }
        public bool IsDone { get; set; }
    }
}
