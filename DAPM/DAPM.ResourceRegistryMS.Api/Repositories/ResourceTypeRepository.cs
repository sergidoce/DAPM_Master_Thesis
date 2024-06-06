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

        public async Task<bool> AddResourceType(ResourceType resourceType)
        {
            await _context.ResourceTypes.AddAsync(resourceType);
            _context.SaveChanges();
            return true;
        }

        public Task<bool> DeleteResourceType(Guid resourceTypeId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResourceType> GetResourceTypeById(Guid id)
        {
            var resourceType = await _context.ResourceTypes.FindAsync(id);

            if (resourceType == null)
            {
                return null;
            }

            return resourceType;
        }

        public Task<IEnumerable<ResourceType>> GetAllResourceTypes()
        {
            throw new NotImplementedException();
        }
    }
}
