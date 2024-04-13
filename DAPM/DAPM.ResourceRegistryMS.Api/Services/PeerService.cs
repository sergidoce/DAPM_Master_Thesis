using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class PeerService : IPeerService
    {
        private IPeerRepository _peerRepository;
        private readonly ILogger<IPeerService> _logger;

        public PeerService(ILogger<IPeerService> logger, IPeerRepository peerRepository)
        {
            _peerRepository = peerRepository;
            _logger = logger;
        }

        public async Task<bool> AddPeer(PeerDto peerDto)
        {
            var peer = new Peer
            {
                Id = peerDto.Id,
                Name = peerDto.Name,
                ApiUrl = peerDto.ApiUrl,
            };

            return await _peerRepository.AddPeer(peer);
        }

        public Task<bool> DeletePeer(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Peer> GetPeer(string id)
        {
            return await _peerRepository.GetPeer(id);
        }

        public Task<IEnumerable<Peer>> GetPeer()
        {
            throw new NotImplementedException();
        }
    }
}
