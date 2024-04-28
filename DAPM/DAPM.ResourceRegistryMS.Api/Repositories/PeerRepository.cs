using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Repositories
{
    public class PeerRepository : IPeerRepository
    {

        private readonly ILogger<IPeerRepository> _logger;
        private readonly ResourceRegistryDbContext _context;

        public PeerRepository(ILogger<IPeerRepository> logger, ResourceRegistryDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<bool> AddPeer(Peer peer)
        {
            await _context.Peers.AddAsync(peer);
            _context.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<Peer>> GetAllPeers()
        {
            return _context.Peers.ToList();
        }

        public async Task<Peer> GetPeer(int id)
        {
            return await _context.Peers.FindAsync(id);
        }
    }
}
