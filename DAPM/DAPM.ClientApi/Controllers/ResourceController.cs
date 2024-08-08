using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DAPM.ClientApi.Controllers
{
   
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("platform/organizations/")]
    public class ResourceController : ControllerBase
    {

        private readonly ILogger<ResourceController> _logger;
        private readonly IResourceService _resourceService;

        public ResourceController(ILogger<ResourceController> logger, IResourceService resourceService)
        {
            _logger = logger;
            _resourceService = resourceService;
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/resources/{resourceId}/GetResourceById")]
        [SwaggerOperation(Description = "Gets a resource by id from a specific repository. The result of this endpoint does not include the resource file. There needs to be " +
            "a collaboration agreement to retrieve this information.")]
        public async Task<ActionResult<Guid>> GetResourceById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            Guid id = _resourceService.GetResourceById(organizationId, repositoryId, resourceId);
            return Ok(new ApiResponse { RequestName = "GetResourceById", TicketId = id });
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/resources/{resourceId}/file/GetResourceFileById")]
        [SwaggerOperation(Description = "Gets a resource file by id from a specific repository. There needs to be " +
            "a collaboration agreement to retrieve this information.")]
        public async Task<ActionResult<Guid>> GetResourceFileById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            Guid id = _resourceService.GetResourceFileById(organizationId, repositoryId, resourceId);
            return Ok(new ApiResponse { RequestName = "GetResourceFileById", TicketId = id });
        }
    }
    
}
