using DAPM.ClientApi.Models;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("operator")]
    public class OperatorController : ControllerBase
    {
        private readonly ILogger<OperatorController> _logger;
        private readonly ITicketService _ticketService;
        private readonly IQueueProducer<OperatorMessage> _operatorMessageProducer;

        public OperatorController(ILogger<OperatorController> logger, ITicketService ticketService, IQueueProducer<OperatorMessage> operatorMessageProducer) 
        {
            _logger = logger;
            _ticketService = ticketService;
            _operatorMessageProducer = operatorMessageProducer;
        }

        [HttpGet]
        public async Task<ActionResult<Guid>> Get(string messageText)
        {
            Guid ticketId = _ticketService.CreateNewTicket();
            var message = new OperatorMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                MessageText = messageText
            };

            _operatorMessageProducer.PublishMessage(message);
            return Ok(new ApiResponse { RequestName = "OperatorMessage", TicketId = ticketId });
        }
    }
}
