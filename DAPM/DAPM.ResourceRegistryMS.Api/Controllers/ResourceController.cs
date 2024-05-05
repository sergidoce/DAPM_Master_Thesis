using Microsoft.AspNetCore.Mvc;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;

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
        public async Task<IActionResult> PostResource(ResourceDto resourceDto)
        {
            var result = await _resourceService.AddResource(resourceDto);

            if (result != null && result == true)
            {
                return Ok();
            }
            else return BadRequest();
                
        }

        [HttpDelete("{resourceId}")]
        public async Task<bool> Delete(int resourceId) 
        {
            return await _resourceService.DeleteResource(resourceId);
        }
    }
}
