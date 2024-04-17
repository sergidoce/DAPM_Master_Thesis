using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages;

namespace DAPM.ClientApi.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ILogger<OrganizationService> _logger;
        private readonly IQueueProducer<GetOrganisationsMessage> _queueProducer;
        private readonly ITicketService _ticketService;

        public OrganizationService(ILogger<OrganizationService> logger, IQueueProducer<GetOrganisationsMessage> queueProducer, ITicketService ticketService)
        {
            _logger = logger;
            _queueProducer = queueProducer;
            _ticketService = ticketService;
        }

        public Guid GetOrganizations()
        {
       
            var ticketId = _ticketService.CreateNewTicket();

            var message = new GetOrganisationsMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
            };

            _queueProducer.PublishMessage(message);

            _logger.LogDebug("Message Enqueued");

            return ticketId;

        }

    }
}
