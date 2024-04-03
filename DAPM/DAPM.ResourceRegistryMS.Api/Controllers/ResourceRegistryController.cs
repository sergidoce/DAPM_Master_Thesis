using Microsoft.AspNetCore.Mvc;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceRegistryController : ControllerBase
    {
        private IResourceRegistryService _resourceRegistryService;

        public ResourceRegistryController(IResourceRegistryService resourceRegistryService) 
        { 
            _resourceRegistryService = resourceRegistryService;
        }

        [HttpGet("{resourceName}")]
        public async Task<Resource> Get(string resourceName)
        {
            return await _resourceRegistryService.GetResource(resourceName);
        }

        [HttpGet]
        public async Task<IEnumerable<Resource>> GetResource() {
            return await _resourceRegistryService.GetResource();
        }

        [HttpDelete("{resourceName}")]
        public async Task<bool> Delete(string resourceName) 
        {
            return await _resourceRegistryService.DeleteResource(resourceName);
        }
    }
}
