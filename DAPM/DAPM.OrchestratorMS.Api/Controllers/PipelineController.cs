using DAPM.OrchestratorMS.Api.Models.Pipeline;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.OrchestratorMS.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PipelineController : ControllerBase
{

    private readonly ILogger<PipelineController> _logger;

    public PipelineController(ILogger<PipelineController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Pipeline pipeline)
    {

        return Ok();
    }
}
