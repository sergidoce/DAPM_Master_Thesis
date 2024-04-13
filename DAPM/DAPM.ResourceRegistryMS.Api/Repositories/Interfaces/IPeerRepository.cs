using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Repositories.Interfaces
{
    public interface IPeerRepository
    {
        public Task<Peer> GetPeer(string id);
        public Task<bool> AddPeer(Peer peer);
    }
}
