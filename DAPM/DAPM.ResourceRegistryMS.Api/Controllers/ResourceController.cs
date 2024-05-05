using Microsoft.AspNetCore.Mvc;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private IResourceService _resourceService;

        public ResourceController(IResourceService resourceService) 
        { 
            _resourceService = resourceService;
        }

        [HttpGet("{resourceId}")]
        public async Task<Resource> Get(int organizationId, int repositoryId, int resourceId)
        {
            return await _resourceService.GetResourceById(organizationId, repositoryId, resourceId);
        }

        [HttpGet]
        public async Task<IEnumerable<Resource>> GetAllResources() {
            return await _resourceService.GetAllResources();
        }

        [HttpPost]
        public async Task<IActionResult> PostResource(ResourceDTO resourceDto)
        {
            return Ok();
                
        }

        [HttpDelete("{resourceId}")]
        public async Task<bool> Delete(int resourceId) 
        {
            return await _resourceService.DeleteResource(resourceId);
        }
    }
}
