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

    }
}
