using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;
using System.IO;
using System.Xml.Linq;

namespace DAPM.ClientApi.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ILogger<RepositoryService> _logger;
        private readonly ITicketService _ticketService;
        IQueueProducer<GetRepositoryByIdMessage> _getRepoByIdProducer;
        IQueueProducer<GetResourcesOfRepositoryMessage> _getResourcesOfRepoProducer;
        IQueueProducer<CreateNewResourceMessage> _createNewResourceProducer;
        IQueueProducer<CreateNewPipelineMessage> _createNewPipelineProducer;

        public RepositoryService(
            ILogger<RepositoryService> logger,
            ITicketService ticketService,
            IQueueProducer<GetRepositoryByIdMessage> getRepoByIdProducer,
            IQueueProducer<GetResourcesOfRepositoryMessage> getResourcesOfRepoProducer,
            IQueueProducer<CreateNewResourceMessage> createNewResourceProducer,
            IQueueProducer<CreateNewPipelineMessage> createNewPipelineProducer) 
        {
            _ticketService = ticketService;
            _logger = logger;
            _getRepoByIdProducer = getRepoByIdProducer;
            _getResourcesOfRepoProducer = getResourcesOfRepoProducer;
            _createNewResourceProducer = createNewResourceProducer;
            _createNewPipelineProducer = createNewPipelineProducer;
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

        public Guid PostPipelineToRepository(int organizationId, int repositoryId, PipelineApiDto pipeline)
        {
            Guid ticketId = _ticketService.CreateNewTicket();

            var message = new CreateNewPipelineMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId,
                Name = pipeline.Name,
                Pipeline = pipeline.Pipeline,

            };

            _createNewPipelineProducer.PublishMessage(message);

            _logger.LogDebug("CreateNewPipelineMessage Enqueued");


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
