using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http.Headers;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Models.DTOs;

namespace DAPM.RepositoryMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceController : ControllerBase
    {

        private readonly ILogger<ResourceController> _logger;
        private IResourceService _resourceService;

        public ResourceController(ILogger<ResourceController> logger, IResourceService resourceService)
        {
            _logger = logger;
            _resourceService = resourceService;
        }

        [HttpGet(Name = "resource")]
        public async Task<FileStreamResult> Get([FromQuery] string name)
        {
            var result = await _resourceService.RetrieveResource(name);
            result.File.Position = 0;
            return new FileStreamResult(result.File, "application/octet-stream")
            {
                FileDownloadName = "resource.csv"
            };
        }

        [HttpPost(Name = "resource")]
        public async Task<ActionResult> Post(ResourceForm resourceForm)
        {

            var filePath = Path.GetTempFileName();
            var fileStream = new FileStream(filePath, FileMode.Create);

            await resourceForm.ResourceFile.CopyToAsync(fileStream);

            var result = await _resourceService.PublishResource(new Resource(resourceForm.Name, fileStream));
            if (result)
            {
                return Ok();
            }
            else return BadRequest();
        }
    }
}
