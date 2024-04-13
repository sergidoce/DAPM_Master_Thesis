using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.ResourceRegistryMS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResourceTypeController : ControllerBase
    {
        private IResourceTypeService _resourceTypeService;
        private readonly ILogger<ResourceTypeController> _logger;

        public ResourceTypeController(ILogger<ResourceTypeController> logger, IResourceTypeService resourceTypeService)
        {
            _resourceTypeService = resourceTypeService;
            _logger = logger;
        }

        [HttpGet("{resourceTypeId}")]
        public async Task<ResourceType> Get(int resourceTypeId)
        {
            return await _resourceTypeService.GetResourceType(resourceTypeId);
        }

        [HttpGet]
        public async Task<IEnumerable<ResourceType>> GetResourceType()
        {
            return await _resourceTypeService.GetResourceType();
        }

        [HttpPost]
        public async Task<IActionResult> PostResource(ResourceTypeDto resourceTypeDto)
        {
            var result = await _resourceTypeService.AddResourceType(resourceTypeDto);

            if (result != null && result == true)
            {
                return Ok();
            }
            else return BadRequest();

        }
    }
}
