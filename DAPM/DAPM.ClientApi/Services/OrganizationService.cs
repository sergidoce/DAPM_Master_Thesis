using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ClientApi.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ILogger<OrganizationService> _logger;
        private readonly IQueueProducer<GetRepositoriesRequest> _getRepositoriesRequestProducer;
        private readonly IQueueProducer<GetOrganizationsRequest> _getOrganizationsRequestProducer;
        private readonly IQueueProducer<PostRepositoryRequest> _postRepositoryRequestProducer;
        private readonly ITicketService _ticketService;

        public OrganizationService(ILogger<OrganizationService> logger, 
            IQueueProducer<GetOrganizationsMessage> getOrgsProducer, 
            IQueueProducer<GetOrganizationsMessage> getOrgByIdProducer,
            IQueueProducer<GetRepositoriesRequest> getRepositoriesRequestProducer,
            IQueueProducer<GetOrganizationsRequest> getOrganizationsRequestProducer,
            IQueueProducer<PostRepositoryRequest> postRepositoryRequestProducer,
            ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
            _getRepositoriesRequestProducer = getRepositoriesRequestProducer;
            _getOrganizationsRequestProducer = getOrganizationsRequestProducer;
            _postRepositoryRequestProducer = postRepositoryRequestProducer;
        }

        public Guid GetOrganizationById(int organizationId)
        {
            var ticketId = _ticketService.CreateNewTicket();

            var message = new GetOrganizationsRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId
            };

            _getOrganizationsRequestProducer.PublishMessage(message);

            _logger.LogDebug("GetOrganizationByIdMessage Enqueued");

            return ticketId;
        }

        public Guid GetOrganizations()
        {
       
            var ticketId = _ticketService.CreateNewTicket();

            var message = new GetOrganizationsRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = null,
            };

            _getOrganizationsRequestProducer.PublishMessage(message);

            _logger.LogDebug("GetOrganizationsMessage Enqueued");

            return ticketId;

        }

        public Guid GetRepositoriesOfOrganization(int organizationId)
        {
            var ticketId = _ticketService.CreateNewTicket();

            var message = new GetRepositoriesRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = null,
            };

            _getRepositoriesRequestProducer.PublishMessage(message);

            _logger.LogDebug("GetRepositoriesRequest Enqueued");

            return ticketId;
        }

        public Guid PostRepositoryToOrganization(int organizationId, string name)
        {
            var ticketId = _ticketService.CreateNewTicket();

            var message = new PostRepositoryRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                Name = name,
            };

            _postRepositoryRequestProducer.PublishMessage(message);

            _logger.LogDebug("PostRepositoryRequest Enqueued");

            return ticketId;
        }
    }
}
