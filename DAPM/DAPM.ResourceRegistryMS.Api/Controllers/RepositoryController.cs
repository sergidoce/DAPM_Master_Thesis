using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.ResourceRegistryMS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private IRepositoryService _repositoryService;
        private readonly ILogger<RepositoryController> _logger;

        public RepositoryController(ILogger<RepositoryController> logger, IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
            _logger = logger;
        }

        [HttpGet("{repositoryId}")]
        public async Task<Repository> Get(int organizationId, int repositoryId)
        {
            return await _repositoryService.GetRepositoryById(organizationId, repositoryId);
        }

        [HttpGet]
        public async Task<IEnumerable<Repository>> GetAllRepositories()
        {
            return await _repositoryService.GetAllRepositories();
        }

        [HttpPost]
        public async Task<IActionResult> PostResource(RepositoryDto repositoryDto)
        {
            var result = await _repositoryService.AddRepository(repositoryDto);

            if (result != null && result == true)
            {
                return Ok();
            }
            else return BadRequest();

        }
    }
}
