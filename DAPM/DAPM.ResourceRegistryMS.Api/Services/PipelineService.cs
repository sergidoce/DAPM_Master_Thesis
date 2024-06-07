using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class PipelineService : IPipelineService
    {
        private IPipelineRepository _pipelineRepository;

        public PipelineService(IPipelineRepository pipelineRepository) 
        {
            _pipelineRepository = pipelineRepository;
        }   
        
        public async Task<Pipeline> GetPipelineById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            return await _pipelineRepository.GetPipelineById(organizationId, repositoryId, resourceId);
        }
    }
}
