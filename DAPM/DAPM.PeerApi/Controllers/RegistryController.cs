using DAPM.PeerApi.Models.ActionsDtos;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Controllers
{
    [ApiController]
    [Route("registry/")]
    public class RegistryController : ControllerBase
    {
        private readonly ILogger<RegistryController> _logger;
        private IQueueProducer<RegistryUpdateMessage> _registryUpdateProducer;

        public RegistryController(ILogger<RegistryController> logger, IQueueProducer<RegistryUpdateMessage> registryUpdateProducer)
        {
            _logger = logger;
        }

        [HttpPost("updates")]
        public async Task<ActionResult> PostRegistryUpdate([FromBody] RegistryUpdateDto registryUpdateDto)
        {
            var registryUpdateDTO = new RegistryUpdateDTO()
            {
                Organizations = registryUpdateDto.Organizations,
                Repositories = registryUpdateDto.Repositories,
                Resources = registryUpdateDto.Resources,
                Pipelines = registryUpdateDto.Pipelines,
            };

            var message = new RegistryUpdateMessage()
            {
                TicketId = (Guid)registryUpdateDto.HandshakeId,
                TimeToLive = TimeSpan.FromMinutes(1),
                RegistryUpdate = registryUpdateDTO,
            };

            _registryUpdateProducer.PublishMessage(message);

            return Ok();
        }
    }
}
