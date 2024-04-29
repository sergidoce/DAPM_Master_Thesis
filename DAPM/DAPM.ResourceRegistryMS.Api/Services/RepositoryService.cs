using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ILogger<IRepositoryService> _logger;
        private IRepositoryRepository _repositoryRepository;
        private IPeerRepository _peerRepository;

        public RepositoryService(ILogger<IRepositoryService> logger, IRepositoryRepository repositoryRepository, IPeerRepository peerRepository)
        {
            _repositoryRepository = repositoryRepository;
            _peerRepository = peerRepository;
            _logger = logger;
        }

        public async Task<bool> AddRepository(RepositoryDto repositoryDto)
        {
            var peer = await _peerRepository.GetPeerById(repositoryDto.Id);

            var repository = new Repository
            {
                Id = repositoryDto.Id,
                Name = repositoryDto.Name,
                Peer = peer
            };

            return await _repositoryRepository.AddRepository(repository);
        }

        public Task<bool> DeleteRepository(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Repository> GetRepository(int id)
        {
            return _repositoryRepository.GetRepositoryById(id);
        }

        public Task<IEnumerable<Repository>> GetRepository()
        {
            throw new NotImplementedException();
        }
    }
}
