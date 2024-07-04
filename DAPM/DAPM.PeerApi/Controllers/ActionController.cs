using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
using DAPM.PipelineOrchestratorMS.Api.Models;
using Microsoft.AspNetCore.Mvc;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;

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

        [HttpPost("transfer-data")]
        public async Task<ActionResult> PostSendDataAction([FromBody] TransferDataActionDto actionDto)
        {
            _actionService.OnTransferDataActionReceived(actionDto.SenderProcessId, actionDto.SenderIdentity, actionDto.StepId, actionDto.Data);
            return Ok();
        }

        [HttpPost("execute-operator")]
        public async Task<ActionResult> PostExecuteOperatorAction([FromBody] ExecuteOperatorActionDto actionDto)
        {
            _actionService.OnExecuteOperatorActionReceived(actionDto.SenderProcessId, actionDto.SenderIdentity, actionDto.StepId, actionDto.Data);
            return Ok();
        }

        [HttpPost("action-result")]
        public async Task<ActionResult> PostActionResult([FromBody] ActionResultDto actionResultDto)
        {
            var actionResultDTO = new ActionResultDTO()
            {
                ActionResult = (PipelineOrchestratorMS.Api.Models.ActionResult)actionResultDto.ActionResult,
                ExecutionId = actionResultDto.ExecutionId,
                StepId = actionResultDto.StepId,
                Message = actionResultDto.Message,
            };

            _actionService.OnActionResultReceived(actionResultDTO);
            return Ok();
        }

    }
}
