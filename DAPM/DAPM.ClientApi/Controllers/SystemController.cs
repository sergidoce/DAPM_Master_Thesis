using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("system")]
    public class SystemController : ControllerBase
    {
        private readonly ILogger<SystemController> _logger;
        private ISystemService _systemService;

        public SystemController(ILogger<SystemController> logger, ISystemService systemService)
        {
            _logger = logger;
            _systemService = systemService;
        }


        //[HttpPost("register-peer")]
        //public async Task<ActionResult<Guid>> RegisterPeer([FromBody] RegisterPeerDto registerPeerDto)
        //{
        //    Guid id = _systemService.RegisterPeer(registerPeerDto.PeerName, registerPeerDto.IntroductionPeerAddress, registerPeerDto.LocalPeerAddress);
        //    return Ok(new ApiResponse { RequestName = "RegisterPeer", TicketId = id });
        //}
    }
}
