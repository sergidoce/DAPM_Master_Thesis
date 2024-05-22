using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Data;

namespace DAPM.RepositoryMS.Api.Repositories
{
    public class ResourceRepository : IResourceRepository
    {

        private ILogger<ResourceRepository> _logger;
        private readonly RepositoryDbContext _repositoryDbContext;
      

        public ResourceRepository(ILogger<ResourceRepository> logger, RepositoryDbContext repositoryDbContext)
        {
            _logger = logger;
            _repositoryDbContext = repositoryDbContext;
        }

        public async Task<int> AddResource(Resource resource)
        {
            await _repositoryDbContext.Resources.AddAsync(resource);
            _repositoryDbContext.SaveChanges();
            return resource.Id;
        }

  
        public Task<Resource> GetResourceById(int repositoryId, int resourceId)
        {
            throw new NotImplementedException();
        }
    }
}
