using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ClientApi.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ILogger<RepositoryService> _logger;
        private readonly ITicketService _ticketService;
        IQueueProducer<GetRepositoryByIdMessage> _getRepoByIdProducer;
        IQueueProducer<GetResourcesOfRepositoryMessage> _getResourcesOfRepoProducer;
        IQueueProducer<CreateNewResourceMessage> _createNewResourceProducer;

        public RepositoryService(
            ILogger<RepositoryService> logger,
            ITicketService ticketService,
            IQueueProducer<GetRepositoryByIdMessage> getRepoByIdProducer,
            IQueueProducer<GetResourcesOfRepositoryMessage> getResourcesOfRepoProducer,
            IQueueProducer<CreateNewResourceMessage> createNewResourceProducer) 
        {
            _ticketService = ticketService;
            _logger = logger;
            _getRepoByIdProducer = getRepoByIdProducer;
            _getResourcesOfRepoProducer = getResourcesOfRepoProducer;
            _createNewResourceProducer = createNewResourceProducer;
        }

        public Guid GetRepositoryById(int organizationId, int repositoryId)
        {
            Guid ticketId = _ticketService.CreateNewTicket();

            var message = new GetRepositoryByIdMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId
            };

            _getRepoByIdProducer.PublishMessage(message);

            _logger.LogDebug("GetRepositoryByIdMessage Enqueued");

            return ticketId;
        }

        public Guid GetResourcesOfRepository(int organizationId, int repositoryId)
        {
            Guid ticketId = _ticketService.CreateNewTicket();

            var message = new GetResourcesOfRepositoryMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId
            };

            _getResourcesOfRepoProducer.PublishMessage(message);

            _logger.LogDebug("GetResourcesOfRepoMessage Enqueued");

            return ticketId;
        }

        public Guid PostResourceToRepository(int organizationId, int repositoryId, string name, IFormFile resourceFile)
        {
            Guid ticketId = _ticketService.CreateNewTicket();

            MemoryStream stream = new MemoryStream();

            resourceFile.CopyTo(stream);


            var message = new CreateNewResourceMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId,
                Name = name,
                ResourceFile = stream.ToArray()

            };

            _createNewResourceProducer.PublishMessage(message);

            _logger.LogDebug("CreateNewResourceMessage Enqueued");

            return ticketId;
        }
    }
}
