using DAPM.ClientApi.Models;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("status")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly ITicketService _ticketService;

        public StatusController(ILogger<StatusController> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        [HttpGet(("{ticketId}"))]
        public async Task<ActionResult<int>> Get(Guid ticketId)
        {
            JToken responseJSON = _ticketService.GetTicketResolution(ticketId);
            var response = responseJSON.ToString();

            return Ok(response);
        }
    }
}
