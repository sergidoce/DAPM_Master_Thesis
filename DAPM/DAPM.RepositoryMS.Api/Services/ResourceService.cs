using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;

namespace DAPM.RepositoryMS.Api.Services
{
    public class ResourceService : IResourceService
    {
        private IResourceRepository _resourceRepository;
        private IOperatorRepository _operatorRepository;
        private IFileRepository _fileRepository;
        private ILogger<ResourceService> _logger;

        public ResourceService(ILogger<ResourceService> logger, IResourceRepository resourceRepository, 
            IFileRepository fileRepository, IOperatorRepository operatorRepository)
        {
            _logger = logger;
            _resourceRepository = resourceRepository;
            _fileRepository = fileRepository;
            _operatorRepository = operatorRepository;
        }

        public async Task<Resource> AddResource(Resource resource)
        {
            return await _resourceRepository.AddResource(resource);
        }

        public async Task<Models.PostgreSQL.File> GetResourceFiles(Guid repositoryId, Guid resourceId)
        {
            return await _resourceRepository.GetResourceFile(repositoryId, resourceId);
        }

        public async Task<Models.PostgreSQL.Resource> GetResourceById(Guid repositoryId, Guid resourceId)
        {
            return await _resourceRepository.GetResourceById(repositoryId, resourceId);
        }

        public async Task<Models.PostgreSQL.Operator> GetOperatorById(Guid repositoryId, Guid resourceId)
        {
            return await _operatorRepository.GetOperatorById(repositoryId, resourceId);
        }

        public async Task<(Models.PostgreSQL.File, Models.PostgreSQL.File)> GetOperatorFiles(Guid repositoryId, Guid resourceId)
        {
            return await _operatorRepository.GetOperatorFiles(repositoryId, resourceId);
        }
    }
}
