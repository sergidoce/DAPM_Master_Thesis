using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.ClientApi.Controllers
{
   
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("organizations/")]
    public class ResourceController : ControllerBase
    {

        private readonly ILogger<ResourceController> _logger;
        private readonly IResourceService _resourceService;

        public ResourceController(ILogger<ResourceController> logger, IResourceService resourceService)
        {
            _logger = logger;
            _resourceService = resourceService;
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/resources/{resourceId}")]
        public async Task<ActionResult<Guid>> GetResourceById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            Guid id = _resourceService.GetResourceById(organizationId, repositoryId, resourceId);
            return Ok(new ApiResponse { RequestName = "GetResourceById", TicketId = id });
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/resources/{resourceId}/file")]
        public async Task<ActionResult<Guid>> GetResourceFileById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            Guid id = _resourceService.GetResourceFileById(organizationId, repositoryId, resourceId);
            return Ok(new ApiResponse { RequestName = "GetResourceFileById", TicketId = id });
        }
    }
    
}
