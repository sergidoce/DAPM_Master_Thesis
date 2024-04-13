using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IPeerService
    {
        Task<Peer> GetPeer(string id);

        Task<IEnumerable<Peer>> GetPeer();
        Task<bool> AddPeer(PeerDto peerDto);

        Task<bool> DeletePeer(string id);
    }
}
