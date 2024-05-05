using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Models.MongoDB;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;

namespace DAPM.RepositoryMS.Api.Services
{
    public class RepositoryService : IRepositoryService
    {
        private ILogger<RepositoryService> _logger;
        private IResourceRepository _resourceRepository;
        private IFileRepository _fileRepository;
        private IRepositoryRepository _repositoryRepository;

        public RepositoryService(ILogger<RepositoryService> logger,
            IResourceRepository resourceRepository,
            IFileRepository fileRepository,
            IRepositoryRepository repositoryRepository)
        {
            _logger = logger;
            _resourceRepository = resourceRepository;
            _fileRepository = fileRepository;
            _repositoryRepository = repositoryRepository;
        }

        public async Task<int> CreateNewResource(int repositoryId, string name, byte[] resourceFile)
        {
            var repository = await _repositoryRepository.GetRepositoryById(repositoryId);

            if(repository != null)
            {
                string objectId = await _fileRepository.AddFile(new MongoFile { Name = name, File = resourceFile });

                if(objectId != null)
                {
                    var file = new Models.PostgreSQL.File
                    {
                        Name = name,
                        MongoDbFileId = objectId,
                        Extension = ".csv"
                    };

                    var resource = new Resource
                    {
                        Name = name,
                        File = file,
                        Repository = repository,
                        Type = "EventLog"
                    };

                    var id = await _resourceRepository.AddResource(resource);

                    return id;
                }
            }

            return -1;
        }
    }
}
