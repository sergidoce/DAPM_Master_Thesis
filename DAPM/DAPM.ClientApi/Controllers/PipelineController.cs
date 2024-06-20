using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PipelineOrchestrator;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("organizations/")]
    public class PipelineController : ControllerBase
    {
        private readonly ILogger<PipelineController> _logger;
        private readonly IPipelineService _pipelineService;
        

        public PipelineController(ILogger<PipelineController> logger, IPipelineService pipelineService, IQueueProducer<CreateInstanceExecutionMessage> createInstanceProducer)
        {
            _logger = logger;
            _pipelineService = pipelineService;
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}")]
        public async Task<ActionResult<Guid>> GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid id = _pipelineService.GetPipelineById(organizationId, repositoryId, pipelineId);
            return Ok(new ApiResponse { RequestName = "GetPipelineById", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}/executions")]
        public async Task<ActionResult<Guid>> CreatePipelineExecutionInstance(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid id = _pipelineService.CreatePipelineExecution(organizationId, repositoryId, pipelineId);
            return Ok(new ApiResponse { RequestName = "CreatePipelineExecutionInstance", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}/executions/{executionId}/commands/start")]
        public async Task<ActionResult<Guid>> PostStartCommand(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId)
        {
            Guid id = _pipelineService.PostStartCommand(organizationId, repositoryId, pipelineId, executionId);
            return Ok(new ApiResponse { RequestName = "PostStartCommand", TicketId = id });
        }
    }
}
