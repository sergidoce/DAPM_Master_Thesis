using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models.HandshakeDtos
{
    public class HandshakeRequestResponseDto
    {
        public IdentityDTO SenderIdentity {  get; set; }
        public Guid HandshakeId { get; set; }
        public bool IsAccepted { get; set; }
    }
}
