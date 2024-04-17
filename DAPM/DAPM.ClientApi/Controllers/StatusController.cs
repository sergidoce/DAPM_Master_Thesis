using DAPM.ClientApi.Models;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly ITicketService _ticketService;

        public StatusController(ILogger<StatusController> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        [HttpGet("status")]
        public async Task<ActionResult<int>> Get(Guid id)
        {
            int response = _ticketService.GetTicketStatus(id);
            return Ok(response);
        }

        [HttpGet("resolution")]
        public async Task<ActionResult<ApiResponse>> GetResolution(Guid id)
        {
            JObject response = _ticketService.GetTicketResolution(id);
            var response_string = response.ToString();

            var responsee = new ApiResponse { response = response_string };

            return Ok(responsee);
        }
    }
}
