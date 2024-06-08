using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.ResourceRegistry;
using System.Globalization;
using System.Resources;

namespace DAPM.ClientApi.Services
{
    public class ResourceService : IResourceService
    {
        private readonly ILogger<ResourceService> _logger;
        private ITicketService _ticketService;

        private IQueueProducer<GetResourcesRequest> _getResourcesRequestProducer;
        private IQueueProducer<GetResourceFilesRequest> _getResourceFilesRequestProducer;

        public ResourceService(ILogger<ResourceService> logger, ITicketService ticketService,
            IQueueProducer<GetResourceFilesRequest> getResourceFilesProducer,
            IQueueProducer<GetResourcesRequest> getResourcesProducer)
        {
            _logger = logger;
            _ticketService = ticketService;
            _getResourceFilesRequestProducer = getResourceFilesProducer;
            _getResourcesRequestProducer = getResourcesProducer;
        }

        public Guid GetResourceById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            var ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new GetResourcesRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId,
                ResourceId = resourceId
            };

            _getResourcesRequestProducer.PublishMessage(message);

            _logger.LogDebug("GetResourcesRequest Enqueued");

            return ticketId;
        }

        public Guid GetResourceFileById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            var ticketId = _ticketService.CreateNewTicket(TicketResolutionType.File);

            var message = new GetResourceFilesRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId,
                ResourceId = resourceId
            };

            _getResourceFilesRequestProducer.PublishMessage(message);

            _logger.LogDebug("GetResourceFilesRequest Enqueued");

            return ticketId;
        }
    }
}
