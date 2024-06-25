using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Data;
using Amazon.Runtime.Internal;

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

        public async Task<Resource> AddResource(Resource resource)
        {
            await _repositoryDbContext.Resources.AddAsync(resource);
            _repositoryDbContext.SaveChanges();
            return resource;
        }

        public async Task<Models.PostgreSQL.File> GetResourceFile(Guid repositoryId, Guid resourceId)
        {
            var resource = _repositoryDbContext.Resources.First(r => r.Id == resourceId && r.RepositoryId == repositoryId);
            return _repositoryDbContext.Files.First(f => f.Id == resource.FileId);
        }

  
        public async Task<Resource> GetResourceById(Guid repositoryId, Guid resourceId)
        {
            return _repositoryDbContext.Resources.First(r => r.Id == resourceId && r.RepositoryId == repositoryId);
        }
    }
}
