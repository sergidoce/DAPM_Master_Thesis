using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using DAPM.ResourceRegistryMS.Api.Resources.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class ResourceRegistryService : IResourceRegistryService
    {
        private IResourceRegistry _resourceRegistry;

        public ResourceRegistryService(IResourceRegistry resourceRegistry) 
        {
            _resourceRegistry = resourceRegistry;
        }

        public async Task<Resource> GetResource(string resourceName)
        {
            return await _resourceRegistry.GetResource(resourceName);
        }

        public async Task<IEnumerable<Resource>> GetResource()
        {
            return await _resourceRegistry.GetResource();
        }

        public async Task<bool> DeleteResource(string resourceName)
        {
            return await _resourceRegistry.DeleteResource(resourceName);
        }
    }
}
