using DAPM.ClientApi.Models;
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("platform/status")]
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
        [SwaggerOperation(Description = "Gets the status of a ticket defined by its id. If the ticket is resolved, it will return the resolution.")]
        public async Task<IActionResult> Get(Guid ticketId)
        {

            TicketResolutionType resolutionType = _ticketService.GetTicketResolutionType(ticketId);

            JToken resolutionJSON = _ticketService.GetTicketResolution(ticketId);

            if(resolutionType == TicketResolutionType.Json || (int)resolutionJSON["status"] != 1)
            {
                var response = resolutionJSON.ToString();
                return Ok(response);
            }
            else
            {
                var filePath = resolutionJSON["result"]["filePath"].ToString();
                var fileName = resolutionJSON["result"]["fileName"].ToString();
                var fileFormat = resolutionJSON["result"]["fileFormat"].ToString();
                var fileContent = await System.IO.File.ReadAllBytesAsync(filePath);

                return File(fileContent, "application/octet-stream", fileName + fileFormat);
            }
        }
    }
}
