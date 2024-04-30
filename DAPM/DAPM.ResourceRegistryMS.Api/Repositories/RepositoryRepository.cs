using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAPM.ResourceRegistryMS.Api.Repositories
{
    public class RepositoryRepository : IRepositoryRepository
    {
        private readonly ResourceRegistryDbContext _context;

        public RepositoryRepository(ResourceRegistryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRepository(Repository repository)
        {
            await _context.Repositories.AddAsync(repository);
            _context.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<Repository>> GetRepositoriesOfOrganization(int organizationId)
        {
            return _context.Repositories.Where(r => r.PeerId == organizationId);   
        }

        public async Task<Repository> GetRepositoryById(int id)
        {
            var repository = _context.Repositories.Include(r => r.Peer).Single(r => r.Id == id);

            if (repository == null)
            {
                return null;
            }

            return repository;
        }
    }
}
