using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.PeerApi.Controllers
{
    [ApiController]
    [Route("actions/")]
    public class ActionController : ControllerBase
    {
        private readonly ILogger<ActionController> _logger;
        private readonly IActionService _actionService;

        public ActionController(ILogger<ActionController> logger, IActionService actionService)
        {
            _logger = logger;
            _actionService = actionService;
        }

        [HttpPost("send-data")]
        public async Task<ActionResult<Guid>> PostSendDataAction([FromBody] SendDataActionDto actionDto)
        {
            throw new NotImplementedException();
        }

        [HttpPost("execute-algorithm")]
        public async Task<ActionResult<Guid>> PostExecuteAlgorithmAction([FromBody] string name)
        {
            throw new NotImplementedException();
        }

        [HttpPost("action-result")]
        public async Task<ActionResult> PostActionResult()
        {
            return Ok();
        }

    }
}
