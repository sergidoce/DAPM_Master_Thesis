using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;

namespace DAPM.RepositoryMS.Api.Services
{
    public class PipelineService : IPipelineService
    {
        private IPipelineRepository _pipelineRepository;
        public PipelineService(IPipelineRepository pipelineRepository) 
        {
            _pipelineRepository = pipelineRepository;
        }
        public Task<Pipeline> GetPipelineById(int repositoryId, int pipelineId)
        {
            return _pipelineRepository.GetPipelineById(repositoryId, pipelineId);
        }
    }
}
