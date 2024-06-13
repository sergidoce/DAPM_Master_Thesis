using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models.HandshakeDtos
{
    public class HandshakeRequestDto
    {
        public IdentityDTO SenderIdentity { get; set; }
        public Guid HandshakeId { get; set; }
    }
}
