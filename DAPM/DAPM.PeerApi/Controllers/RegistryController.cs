using DAPM.PeerApi.Models;
using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
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
        private IRegistryService _registryService;
        private IQueueProducer<RegistryUpdateMessage> _registryUpdateProducer;

        public RegistryController(ILogger<RegistryController> logger, 
            IQueueProducer<RegistryUpdateMessage> registryUpdateProducer,
            IRegistryService registryService)
        {
            _logger = logger;
            _registryService = registryService;
            _registryUpdateProducer = registryUpdateProducer;
        }

        [HttpPost("updates")]
        public async Task<ActionResult> PostRegistryUpdate([FromBody] RegistryUpdateDto registryUpdateDto)
        {
            _registryService.OnRegistryUpdate(registryUpdateDto);
            return Ok("Registry update received");
        }

        [HttpPost("update-ack")]
        public async Task<ActionResult> PostRegistryUpdateAck([FromBody] RegistryUpdateAckDto registryUpdateAckDto)
        {
            _registryService.OnRegistryUpdateAck(registryUpdateAckDto);
            return Ok("RegistryUpdate ack received");
        }
    }
}
