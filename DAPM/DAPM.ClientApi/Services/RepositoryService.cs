using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
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
        IQueueProducer<GetRepositoriesRequest> _getRepositoriesRequestProducer;
        IQueueProducer<GetResourcesRequest> _getResourcesRequestProducer;
        IQueueProducer<PostResourceRequest> _postResourceRequestProducer;
        IQueueProducer<PostPipelineRequest> _postPipelineRequestProducer;

        public RepositoryService(
            ILogger<RepositoryService> logger,
            ITicketService ticketService,
            IQueueProducer<GetRepositoriesRequest> getRepositoriesRequestProducer,
            IQueueProducer<GetResourcesRequest> getResourcesRequestProducer,
            IQueueProducer<PostResourceRequest> postResourceRequestProducer,
            IQueueProducer<PostPipelineRequest> postPipelineRequestProducer) 
        {
            _ticketService = ticketService;
            _logger = logger;
            _getRepositoriesRequestProducer = getRepositoriesRequestProducer;
            _getResourcesRequestProducer = getResourcesRequestProducer;
            _postResourceRequestProducer = postResourceRequestProducer;
            _postPipelineRequestProducer = postPipelineRequestProducer;
        }

        public Guid GetRepositoryById(Guid organizationId, Guid repositoryId)
        {
            Guid ticketId = _ticketService.CreateNewTicket();

            var message = new GetRepositoriesRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId
            };

            _getRepositoriesRequestProducer.PublishMessage(message);

            _logger.LogDebug("GetRepositoriesRequest Enqueued");

            return ticketId;
        }

        public Guid GetResourcesOfRepository(Guid organizationId, Guid repositoryId)
        {
            Guid ticketId = _ticketService.CreateNewTicket();

            var message = new GetResourcesRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId
            };

            _getResourcesRequestProducer.PublishMessage(message);

            _logger.LogDebug("GetResourcesRequest Enqueued");

            return ticketId;
        }

        public Guid PostPipelineToRepository(Guid organizationId, Guid repositoryId, PipelineApiDto pipeline)
        {
            Guid ticketId = _ticketService.CreateNewTicket();

            var message = new PostPipelineRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId,
                Name = pipeline.Name,
                Pipeline = pipeline.Pipeline,

            };

            _postPipelineRequestProducer.PublishMessage(message);

            _logger.LogDebug("PostPipelineToRepoMessage Enqueued");


            return ticketId;
        }

        public Guid PostResourceToRepository(Guid organizationId, Guid repositoryId, string name, IFormFile resourceFile)
        {
            Guid ticketId = _ticketService.CreateNewTicket();

            MemoryStream stream = new MemoryStream();

            resourceFile.CopyTo(stream);


            var message = new PostResourceRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId,
                Name = name,
                ResourceFile = stream.ToArray()

            };

            _postResourceRequestProducer.PublishMessage(message);

            _logger.LogDebug("PostResourceRequest Enqueued");

            return ticketId;
        }
    }
}
