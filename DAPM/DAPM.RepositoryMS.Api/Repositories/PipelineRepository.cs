using DAPM.RepositoryMS.Api.Data;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAPM.RepositoryMS.Api.Repositories
{
    public class PipelineRepository : IPipelineRepository
    {
        private ILogger<PipelineRepository> _logger;
        private readonly RepositoryDbContext _repositoryDbContext;

        public PipelineRepository(ILogger<PipelineRepository> logger,  RepositoryDbContext repositoryDbContext)
        {
            _logger = logger;
            _repositoryDbContext = repositoryDbContext;
        }

        public async Task<Pipeline> AddPipeline(Pipeline pipeline)
        {
            await _repositoryDbContext.Pipelines.AddAsync(pipeline);
            _repositoryDbContext.SaveChanges();
            return pipeline;
        }

        public async Task<Pipeline> GetPipelineById(int repositoryId, int pipelineId)
        {
            return await _repositoryDbContext.Pipelines.FirstOrDefaultAsync(p => p.Id == pipelineId && p.RepositoryId == repositoryId);
        }
    }
}
