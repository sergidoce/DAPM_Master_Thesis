using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;

namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class ResourceService : IResourceService
    {
        private IResourceRepository _resourceRepository;
        private IRepositoryRepository _repositoryRepository;
        private IResourceTypeRepository _resourceTypeRepository;
        private readonly ILogger<IResourceService> _logger;

        public ResourceService(ILogger<IResourceService> logger, IResourceRepository resourceRepository, IRepositoryRepository repositoryRepository, IResourceTypeRepository resourceTypeRepository) 
        {
            _resourceRepository = resourceRepository;
            _repositoryRepository = repositoryRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _logger = logger;
        }

        public async Task<Resource> GetResource(int resourceId)
        {
            return await _resourceRepository.GetResourceById(resourceId);
        }

        public async Task<IEnumerable<Resource>> GetResource()
        {
            return await _resourceRepository.GetAllResources();
        }

        public async Task<bool> AddResource(ResourceDto resourceDto)
        {
            var repositoryId = resourceDto.RepositoryId;
            var resourceTypeId = resourceDto.TypeId;

            var repository = await _repositoryRepository.GetRepositoryById(repositoryId);
            var resourceType = await _resourceTypeRepository.GetResourceTypeById(resourceTypeId);

            var resource = new Resource
            {
                Id = resourceDto.Id,
                Name = resourceDto.Name,
                Repository = repository,
                Type = resourceType
            };

            await _resourceRepository.AddResource(resource);

            return true;
        }

        public async Task<bool> DeleteResource(int resourceId)
        {
            return await _resourceRepository.DeleteResource(resourceId);
        }
    }
}
