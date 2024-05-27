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
        IQueueProducer<CreateInstanceExecutionMessage> _createInstanceProducer;

        public PipelineController(ILogger<PipelineController> logger, IPipelineService pipelineService, IQueueProducer<CreateInstanceExecutionMessage> createInstanceProducer)
        {
            _logger = logger;
            _pipelineService = pipelineService;
            _createInstanceProducer = createInstanceProducer;
        }

        [HttpPost("/pipelineExecutionTest")]
        public async Task<ActionResult<Guid>> PostPipelineToRepository([FromBody] PipelineApiDto pipelineApiDto)
        {
            Guid id = Guid.NewGuid();

            var message = new CreateInstanceExecutionMessage()
            {
                TicketId = id,
                TimeToLive = TimeSpan.FromMinutes(1),
                Pipeline = pipelineApiDto.Pipeline,
            };

            _createInstanceProducer.PublishMessage(message);

            return Ok(new ApiResponse { RequestName = "ExecutePipeline", TicketId = id });
        }
    }
}
