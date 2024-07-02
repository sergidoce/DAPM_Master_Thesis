using DAPM.PeerApi.Models;
using DAPM.PeerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;

namespace DAPM.PeerApi.Controllers
{
    [ApiController]
    [Route("resources/")]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;
        private IQueueProducer<PostResourceFromPeerRequest> _postResourceQueueProducer;
        private IQueueProducer<SendResourceToPeerResultMessage> _sendResourceToPeerResultQueueProducer;

        public ResourceController(ILogger<ResourceController> logger,
            IQueueProducer<PostResourceFromPeerRequest> postResourceQueueProducer,
            IQueueProducer<SendResourceToPeerResultMessage> sendResourceToPeerResultQueueProducer)
        {
            _logger = logger;
            _postResourceQueueProducer = postResourceQueueProducer;
            _sendResourceToPeerResultQueueProducer = sendResourceToPeerResultQueueProducer;
        }

        [HttpPost]
        public async Task<ActionResult> PostResource([FromBody] SendResourceToPeerDto sendResourceToPeerDto)
        {
            _logger.LogInformation("Ticket id / step id in post resource endpoint is " + sendResourceToPeerDto.StepId.ToString());
            var message = new PostResourceFromPeerRequest()
            {
                ExecutionId = sendResourceToPeerDto.ExecutionId,
                TicketId = sendResourceToPeerDto.StepId,
                RepositoryId = sendResourceToPeerDto.RepositoryId,
                Resource = sendResourceToPeerDto.Resource,
                SenderPeerIdentity = sendResourceToPeerDto.SenderPeerIdentity,
                StorageMode = sendResourceToPeerDto.StorageMode,
                TimeToLive = TimeSpan.FromMinutes(1),
            };

            _postResourceQueueProducer.PublishMessage(message);
            return Ok("Post resource received");
        }

        [HttpPost("result")]
        public async Task<ActionResult> PostResourceResult([FromBody] SendResourceToPeerResultDto sendResourceToPeerResultDto)
        {
            _logger.LogInformation("Ticket id / step id in post resource result endpoint is " + sendResourceToPeerResultDto.StepId.ToString());
            var message = new SendResourceToPeerResultMessage()
            {
                TicketId = sendResourceToPeerResultDto.StepId,
                Succeeded = sendResourceToPeerResultDto.Succeeded,
                TimeToLive = TimeSpan.FromMinutes(1),
            };

            _sendResourceToPeerResultQueueProducer.PublishMessage(message);
            return Ok("Post resource result received");
        }
    }
}
