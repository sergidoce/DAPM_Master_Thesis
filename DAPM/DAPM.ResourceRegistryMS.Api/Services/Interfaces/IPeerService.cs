using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Models;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IPeerService
    {
        Task<Peer> GetPeer(int id);

        Task<IEnumerable<Peer>> GetAllPeers();
        Task<bool> AddPeer(PeerDto peerDto);

        Task<bool> DeletePeer(int id);

        Task<IEnumerable<Repository>> GetRepositoriesOfOrganization(int organizationId);
        Task<Repository> PostRepositoryToOrganization(int organizationId, RepositoryDTO repository); 
    }
}
