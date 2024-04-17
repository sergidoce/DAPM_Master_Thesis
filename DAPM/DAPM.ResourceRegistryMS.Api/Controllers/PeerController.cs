using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace DAPM.ResourceRegistryMS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeerController : ControllerBase
    {
        private IPeerService _peerService;
        private readonly ILogger<PeerController> _logger;

        public PeerController(ILogger<PeerController> logger, IPeerService peerService)
        {
            _peerService = peerService;
            _logger = logger;
        }

        [HttpGet("{peerId}")]
        public async Task<Peer> Get(string peerId)
        {
            return await _peerService.GetPeer(peerId);
        }

        [HttpGet]
        public async Task<IEnumerable<Peer>> GetPeer()
        {
            return await _peerService.GetPeer();
        }

        [HttpPost]
        public async Task<IActionResult> PostResource(PeerDto peerDto)
        {
            var result = await _peerService.AddPeer(peerDto);

            if (result != null && result == true)
            {
                return Ok();
            }
            else return BadRequest();

        }
    }
}
