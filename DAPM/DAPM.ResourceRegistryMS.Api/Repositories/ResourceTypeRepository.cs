using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Repositories
{
    public class ResourceTypeRepository : IResourceTypeRepository
    {
        private readonly ResourceRegistryDbContext _context;

        public ResourceTypeRepository(ResourceRegistryDbContext context)
        {
            _context = context;
        }

        public async Task<ResourceType> GetResourceType(string id)
        {
            var resourceType = await _context.ResourceTypes.FindAsync(id);

            if (resourceType == null)
            {
                return null;
            }

            return resourceType;
        }
    }
}
