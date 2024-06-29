using DAPM.PeerApi.Models;
using DAPM.PeerApi.Models.ActionsDtos;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Services.Interfaces
{
    public interface IRegistryService
    {
        public void OnRegistryUpdateAck(RegistryUpdateAckDto handshakeAck);
        public void OnRegistryUpdate(RegistryUpdateDto registryUpdateDto);


    }
}
