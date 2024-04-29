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
        public async Task<Repository> Get(int repositoryId)
        {
            return await _repositoryService.GetRepository(repositoryId);
        }

        [HttpGet]
        public async Task<IEnumerable<Repository>> GetRepository()
        {
            return await _repositoryService.GetRepository();
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
