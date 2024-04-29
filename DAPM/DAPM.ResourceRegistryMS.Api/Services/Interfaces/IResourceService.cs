using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IResourceService
    {
        Task<Resource> GetResource(int id);

        Task<IEnumerable<Resource>> GetResource();
        Task<bool> AddResource(ResourceDto resource);  

        Task<bool> DeleteResource(int id);
    }
}
