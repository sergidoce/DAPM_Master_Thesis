using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ClientApi.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ILogger<OrganizationService> _logger;
        private readonly IQueueProducer<GetOrganizationsMessage> _getOrgsProducer;
        private readonly IQueueProducer<GetOrganizationByIdMessage> _getOrgByIdProducer;
        private readonly IQueueProducer<GetUsersOfOrganizationMessage> _getUsersOfOrgProducer;
        private readonly ITicketService _ticketService;

        public OrganizationService(ILogger<OrganizationService> logger, 
            IQueueProducer<GetOrganizationsMessage> getOrgsProducer, 
            IQueueProducer<GetOrganizationByIdMessage> getOrgByIdProducer,
            IQueueProducer<GetUsersOfOrganizationMessage>getUsersOfOrgProducer,
            ITicketService ticketService)
        {
            _logger = logger;
            _getOrgsProducer = getOrgsProducer;
            _getOrgByIdProducer = getOrgByIdProducer;
            _ticketService = ticketService;
            _getUsersOfOrgProducer = getUsersOfOrgProducer;
        }

        public Guid GetOrganizationById(int organizationId)
        {
            var ticketId = _ticketService.CreateNewTicket();

            var message = new GetOrganizationByIdMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId
            };

            _getOrgByIdProducer.PublishMessage(message);

            _logger.LogDebug("Message Enqueued");

            return ticketId;
        }

        public Guid GetOrganizations()
        {
       
            var ticketId = _ticketService.CreateNewTicket();

            var message = new GetOrganizationsMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
            };

            _getOrgsProducer.PublishMessage(message);

            _logger.LogDebug("Message Enqueued");

            return ticketId;

        }

        public Guid GetUsersOfOrganization(int organizationId)
        {
            var ticketId = _ticketService.CreateNewTicket();

            var message = new GetUsersOfOrganizationMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId
            };

            _getUsersOfOrgProducer.PublishMessage(message);

            _logger.LogDebug("Message Enqueued");

            return ticketId;
        }
    }
}
