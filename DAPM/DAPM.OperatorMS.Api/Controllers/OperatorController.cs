using Microsoft.AspNetCore.Mvc;
using DAPM.OperatorMS.Api.Services.Interfaces;

namespace DAPM.OperatorMS.Api.Controllers
{
    [ApiController]
    [Route("Operator")]
    public class OperatorController : ControllerBase
    {
        private readonly ILogger<OperatorController> _logger;
        private IOperatorService _operatorService;

        public OperatorController(ILogger<OperatorController> logger, IOperatorService operatorService) { 
            _logger = logger;
            _operatorService = operatorService;
        }

        [HttpPost(Name = "operator")]
        public async Task<IActionResult> Get(IFormFile event_log, IFormFile sourceCode, IFormFile dockerFile) {

            string imageName = await _operatorService.ExecuteMiner(event_log, sourceCode, dockerFile);

            return Ok(imageName);
            
            //byte[] pngImageBytes = await _operatorService.ExecuteMiner(operatorName, parameter);

            //return File(pngImageBytes, "image/png");
        }
    }
}
