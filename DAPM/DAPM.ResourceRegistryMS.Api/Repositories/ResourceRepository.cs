using Microsoft.EntityFrameworkCore;
using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ResourceRegistryDbContext _context;

        public ResourceRepository(ResourceRegistryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resource>> GetAllResources()
        {
            return await _context.Resources.ToListAsync();
        }

        public async Task<bool> AddResource(Resource resource)
        {
            await _context.Resources.AddAsync(resource);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteResource(Guid resourceId)
        {
            var resource = await _context.Resources.FindAsync(resourceId);

            if (resource == null)
            {
                return false;
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Resource> GetResourceById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            return (Resource)_context.Resources.Where(r => r.PeerId == organizationId && r.RepositoryId == repositoryId && r.Id == resourceId);
        }

        public IEnumerable<Resource> GetResourcesOfRepository(Guid organizationId, Guid repositoryId)
        {
            return _context.Resources.Where(r => r.PeerId == organizationId && r.RepositoryId == repositoryId);
        }

    }
}
