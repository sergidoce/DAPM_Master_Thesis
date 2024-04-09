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

        public async Task<Repository> GetRepository(string id)
        {
            var repository = await _context.Repositories.FindAsync(id);

            if (repository == null)
            {
                return null;
            }

            return repository;
        }
    }
}
