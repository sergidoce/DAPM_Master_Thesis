using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("organizations/")]
    public class PipelineController : ControllerBase
    {
        private readonly ILogger<PipelineController> _logger;
        private readonly IPipelineService _pipelineService;

        public PipelineController(ILogger<PipelineController> logger, IPipelineService pipelineService)
        {
            _logger = logger;
            _pipelineService = pipelineService;
        }
    }
}
