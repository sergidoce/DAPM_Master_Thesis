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
        public async Task<IActionResult> Post(IFormFile event_log, IFormFile dockerFile, IFormFile sourceCodeFile)
        {
            byte[] fileBytes = await _operatorService.ExecuteMiner(event_log, dockerFile, sourceCodeFile);
            string fileName = "output.html";

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
