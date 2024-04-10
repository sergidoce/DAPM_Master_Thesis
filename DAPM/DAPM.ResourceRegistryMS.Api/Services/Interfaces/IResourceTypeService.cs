using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IResourceTypeService
    {
        Task<ResourceType> GetResourceType(int id);

        Task<IEnumerable<ResourceType>> GetResourceType();
        Task<bool> AddResourceType(ResourceTypeDto resourceTypeDto);
        Task<bool> DeleteResourceType(int id);
    }
}
