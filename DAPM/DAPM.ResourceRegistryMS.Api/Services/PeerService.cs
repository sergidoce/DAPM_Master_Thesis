using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class PeerService : IPeerService
    {
        private IPeerRepository _peerRepository;
        private IRepositoryRepository _repositoryRepository;
        private readonly ILogger<IPeerService> _logger;

        public PeerService(ILogger<IPeerService> logger, IPeerRepository peerRepository, IRepositoryRepository repositoryRepository)
        {
            _peerRepository = peerRepository;
            _repositoryRepository = repositoryRepository;
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

        public Task<bool> DeletePeer(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Peer> GetPeer(int id)
        {
            return await _peerRepository.GetPeerById(id);
        }

        public async Task<IEnumerable<Peer>> GetAllPeers()
        {
            return await _peerRepository.GetAllPeers();
        }

        public async Task<IEnumerable<Repository>> GetRepositoriesOfOrganization(int organizationId)
        {
            return await _repositoryRepository.GetRepositoriesOfOrganization(organizationId); 
        }

        public async Task<Repository> PostRepositoryToOrganization(int organizationId, RepositoryDTO repositoryDTO)
        {
            Repository repository = new Repository()
            {
                Id = repositoryDTO.Id,
                Name = repositoryDTO.Name,
                PeerId = organizationId,
            };

            return await _repositoryRepository.PostRepository(repository);
        }
    }
}
