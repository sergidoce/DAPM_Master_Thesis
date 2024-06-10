using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IPeerRepository
    {
        public Task<IEnumerable<Peer>> GetAllPeers();
        public Task<Peer> GetPeerById(Guid id);
        public Task<bool> AddPeer(Peer peer);
    }
}
