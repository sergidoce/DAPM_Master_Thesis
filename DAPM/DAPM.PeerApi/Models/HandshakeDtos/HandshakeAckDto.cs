using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models.HandshakeDtos
{
    public class HandshakeAckDto
    {
        public IdentityDTO SenderIdentity { get; set; }
        public Guid HandshakeId { get; set; }
        public bool IsDone { get; set; }
    }
}
