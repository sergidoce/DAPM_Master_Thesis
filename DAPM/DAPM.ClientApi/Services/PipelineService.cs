using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ClientApi.Services
{
    public class PipelineService : IPipelineService
    {
        private readonly ILogger<PipelineService> _logger;
        private readonly ITicketService _ticketService;
        private readonly IQueueProducer<GetPipelinesRequest> _getPipelinesRequestProducer;

        public PipelineService(
            ILogger<PipelineService> logger,
            ITicketService ticketService,
            IQueueProducer<GetPipelinesRequest> getPipelinesRequestProducer)
        {
            _logger = logger;
            _ticketService = ticketService;
            _getPipelinesRequestProducer = getPipelinesRequestProducer;
        }

        public Guid GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid ticketId = _ticketService.CreateNewTicket();

            var message = new GetPipelinesRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId,
                PipelineId = pipelineId
            };

            _getPipelinesRequestProducer.PublishMessage(message);

            _logger.LogDebug("GetPipelinesRequest Enqueued");

            return ticketId;
        }
    }
}
