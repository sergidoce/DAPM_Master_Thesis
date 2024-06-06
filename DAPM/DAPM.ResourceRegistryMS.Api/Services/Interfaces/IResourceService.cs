using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IResourceService
    {
        Task<Resource> GetResourceById(Guid organizationId, Guid repositoryId, Guid resourceId);
        Task<Resource> AddResource(RabbitMQLibrary.Models.ResourceDTO resourceDto);  
        Task<IEnumerable<Resource>> GetAllResources();
        Task<bool> DeleteResource(Guid id);
    }
}
