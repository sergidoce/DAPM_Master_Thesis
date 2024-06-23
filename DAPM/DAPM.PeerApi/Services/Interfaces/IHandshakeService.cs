using DAPM.PeerApi.Models.HandshakeDtos;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Services.Interfaces
{
    public interface IHandshakeService
    {
        public void OnHandshakeRequest(Guid handshakeId, IdentityDTO senderIdentity);
        public void OnHandshakeRequestResponse(Guid handshakeId, IdentityDTO senderIdentity, bool IsAccepted);
    }
}
