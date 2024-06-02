using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IResourceService
    {
        Task<Resource> GetResourceById(int organizationId, int repositoryId, int resourceId);
        Task<Resource> AddResource(RabbitMQLibrary.Models.ResourceDTO resourceDto);  
        Task<IEnumerable<Resource>> GetAllResources();
        Task<bool> DeleteResource(int id);
    }
}
