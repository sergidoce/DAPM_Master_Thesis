using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IResourceService
    {
        Task<Resource> GetResourceById(int organizationId, int repositoryId, int resourceId);
        Task<bool> AddResource(ResourceDto resource);  
        Task<IEnumerable<Resource>> GetAllResources();
        Task<bool> DeleteResource(int id);
    }
}
