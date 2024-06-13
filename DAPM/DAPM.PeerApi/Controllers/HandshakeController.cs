using DAPM.PeerApi.Models.HandshakeDtos;
using DAPM.PeerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.PeerApi.Controllers
{
    [ApiController]
    [Route("handshake/")]
    public class HandshakeController : ControllerBase
    {
        private readonly ILogger<HandshakeController> _logger;
        private IHandshakeService _handshakeService;

        public HandshakeController(ILogger<HandshakeController> logger, IHandshakeService handshakeService)
        {
            _logger = logger;
            _handshakeService = handshakeService;
        }

        [HttpPost("request")]
        public async Task<ActionResult> PostHandshakeRequest([FromBody] HandshakeRequestDto requestDto)
        {
            _handshakeService.OnHandshakeRequest(requestDto.HandshakeId, requestDto.SenderIdentity);
            return Ok("Handshake request received");
        }

        [HttpPost("request-response")]
        public async Task<ActionResult> PostHandshakeRequestResponse([FromBody] HandshakeRequestResponseDto requestResponseDto)
        {
            _handshakeService.OnHandshakeRequestResponse(requestResponseDto.HandshakeId, 
                requestResponseDto.SenderIdentity, requestResponseDto.IsAccepted);
            return Ok("Handshake request response received");
        }

        [HttpPost("ack")]
        public async Task<ActionResult> PostHandshakeAck([FromBody] HandshakeAckDto handshakeAckDto)
        {
            _handshakeService.OnHandshakeAck(handshakeAckDto.HandshakeId,
                handshakeAckDto.SenderIdentity, handshakeAckDto);
            return Ok("Handshake ack received");
        }
    }
}
