using DAPM.ClientApi.Models;
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("organizations")]
    public class OrganizationController : ControllerBase
    {

        private readonly ILogger<OrganizationController> _logger;
        private readonly IOrganizationService _organizationService;

        public OrganizationController(ILogger<OrganizationController> logger, IOrganizationService organizationService)
        {
            _logger = logger;
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<ActionResult<Guid>> Get()
        {
            Guid id = _organizationService.GetOrganizations();
            return Ok(new ApiResponse { RequestName = "GetAllOrganizations", TicketId = id});
        }

        [HttpGet("{organizationId}")]
        public async Task<ActionResult<Guid>> GetById(Guid organizationId)
        {
            Guid id = _organizationService.GetOrganizationById(organizationId);
            return Ok(new ApiResponse { RequestName = "GetOrganizationById", TicketId = id });
        }

        [HttpGet("{organizationId}/repositories")]
        public async Task<ActionResult<Guid>> GetRepositoriesOfOrganization(Guid organizationId)
        {
            Guid id = _organizationService.GetRepositoriesOfOrganization(organizationId);
            return Ok(new ApiResponse {RequestName = "GetRepositoriesOfOrganization", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories")]
        public async Task<ActionResult<Guid>> PostRepositoryToOrganization(Guid organizationId, [FromBody] string name)
        {
            Guid id = _organizationService.PostRepositoryToOrganization(organizationId, name);
            return Ok(new ApiResponse { RequestName = "PostRepositoryToOrganization", TicketId = id });
        }

    }
}
