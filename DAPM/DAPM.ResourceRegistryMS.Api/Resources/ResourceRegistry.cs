using Microsoft.EntityFrameworkCore;
using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Resources.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Resources
{
    public class ResourceRegistry : IResourceRegistry
    {
        private readonly ResourceRegistryDbContext _context;

        public ResourceRegistry(ResourceRegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Resource> GetResource(string resourceName) 
        {
            var resource = await _context.Resources.FindAsync(resourceName);

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
