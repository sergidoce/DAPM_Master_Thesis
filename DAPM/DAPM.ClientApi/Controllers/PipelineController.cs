using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using Swashbuckle.AspNetCore.Annotations;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [EnableCors("AllowAll")]
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
        [SwaggerOperation(Description = "Gets a pipeline by id. This endpoint includes the " +
            "pipeline model in JSON. You need to have a collaboration agreement to retrieve this information.")]
        public async Task<ActionResult<Guid>> GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid id = _pipelineService.GetPipelineById(organizationId, repositoryId, pipelineId);
            return Ok(new ApiResponse { RequestName = "GetPipelineById", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}/executions")]
        [SwaggerOperation(Description = "Creates a new execution instance for a pipeline previously saved in the system. The execution is created but not started")]
        public async Task<ActionResult<Guid>> CreatePipelineExecutionInstance(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid id = _pipelineService.CreatePipelineExecution(organizationId, repositoryId, pipelineId);
            return Ok(new ApiResponse { RequestName = "CreatePipelineExecutionInstance", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}/executions/{executionId}/commands/start")]
        [SwaggerOperation(Description = "Posts a start command to the defined pipeline execution. The start command will start the pipeline execution.")]
        public async Task<ActionResult<Guid>> PostStartCommand(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId)
        {
            Guid id = _pipelineService.PostStartCommand(organizationId, repositoryId, pipelineId, executionId);
            return Ok(new ApiResponse { RequestName = "PostStartCommand", TicketId = id });
        }
    }
}
