using DAPM.ClientApi.Models.DTOs;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IRepositoryService
    {
        public Guid GetRepositoryById(Guid organizationId, Guid repositoryId);
        public Guid GetResourcesOfRepository(Guid organizationId, Guid repositoryId);
        public Guid PostResourceToRepository(Guid organizationId, Guid repositoryId, string name, IFormFile resourceFile);
        public Guid PostPipelineToRepository(Guid organizationId, Guid repositoryId, PipelineApiDto pipeline);
    }
}
