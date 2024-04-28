using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("controller")]
    public class OperatorController : ControllerBase
    {
        private readonly ILogger<OperatorController> _logger;
        private readonly IOperatorService _operatorService;

        public OperatorController(ILogger<OperatorController> logger, IOperatorService operatorService)
        {
            _logger = logger;
            _operatorService = operatorService;
        }

        [HttpGet(Name = "operator")]
        public async Task<ActionResult<Guid>> Get([FromQuery] string minerId, [FromQuery] string resourceId)
        {
            Guid id = _operatorService.ExecuteOperator(minerId, resourceId);
            return Ok(id);
        }
    }
}
