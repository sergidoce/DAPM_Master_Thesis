using Microsoft.AspNetCore.Mvc;
using DAPM.OperatorMS.Api.Services.Interfaces;

namespace DAPM.OperatorMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperatorController : ControllerBase
    {
        private readonly ILogger<OperatorController> _logger;
        private IOperatorService _operatorService;

        public OperatorController(ILogger<OperatorController> logger, IOperatorService operatorService) { 
            _logger = logger;
            _operatorService = operatorService;
        }

        [HttpGet(Name = "operator")]
        public async Task<IActionResult> Get([FromQuery] string operatorName, [FromQuery] string parameter) {
            byte[] pngImageBytes = await _operatorService.ExecuteMiner(operatorName, parameter);

            return File(pngImageBytes, "image/png");
        }
    }
}
