using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IResourceRegistryService
    {
        Task<Resource> GetResource(string name);

        Task<IEnumerable<Resource>> GetResource();

        Task<bool> DeleteResource(string name);
    }
}
