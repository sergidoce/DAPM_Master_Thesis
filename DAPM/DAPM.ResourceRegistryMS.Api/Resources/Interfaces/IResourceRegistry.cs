using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Resources.Interfaces
{
    public interface IResourceRegistry
    {
        public Task<Resource> GetResource(string resourceName);
        public Task<IEnumerable<Resource>> GetResource();
        public Task<bool> DeleteResource(string resourceName);

    }
}
