using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages;

namespace DAPM.ClientApi.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly ILogger<OperatorService> _logger;
        private readonly IQueueProducer<GetExecutionMessage> _queueProducer;
        private readonly ITicketService _ticketService;

        public OperatorService(ILogger<OperatorService> logger, IQueueProducer<GetExecutionMessage> queueProducer, ITicketService ticketService)
        {
            _logger = logger;
            _queueProducer = queueProducer;
            _ticketService = ticketService;
        }

        public Guid ExecuteOperator(string minerId, string resourceId) 
        { 
            var ticketId = _ticketService.CreateNewTicket();

            var message = new GetExecutionMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                ResourceId = resourceId,
                MinerId = minerId
            };

            _queueProducer.PublishMessage(message);

            _logger.LogDebug("Message Enqueued");

            return ticketId; 
        }
    }
}
