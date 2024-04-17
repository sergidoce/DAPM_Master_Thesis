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

        public async Task<Resource> GetResource(string resourceId) 
        {
            var resource = await _context.Resources.FindAsync(resourceId);

            if (resource == null)
            {
                return new Resource { Name = "Resource Not Found" };
            }

            return resource;
        }

        public async Task<IEnumerable<Resource>> GetResource()
        {
            return await _context.Resources.ToListAsync();
        }

        public async Task<bool> AddResource(Resource resource)
        {
            await _context.Resources.AddAsync(resource);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteResource(string resourceName)
        {
            var resource = await _context.Resources.FindAsync(resourceName);

            if (resource == null)
            {
                return false;
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
