using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ClientApi.Services
{
    public class PipelineService : IPipelineService
    {
        private readonly ILogger<PipelineService> _logger;
        private readonly ITicketService _ticketService;
        private readonly IQueueProducer<GetPipelinesRequest> _getPipelinesRequestProducer;
        private readonly IQueueProducer<CreatePipelineExecutionRequest> _createInstanceProducer;
        private readonly IQueueProducer<PipelineStartCommandRequest> _pipelineStartCommandProducer;
        private readonly IQueueProducer<GetPipelineExecutionStatusRequest> _getPipelineExecutionStatusProducer;
        public PipelineService(
            ILogger<PipelineService> logger,
            ITicketService ticketService,
            IQueueProducer<GetPipelinesRequest> getPipelinesRequestProducer,
            IQueueProducer<CreatePipelineExecutionRequest> createInstanceProducer,
            IQueueProducer<PipelineStartCommandRequest> pipelineStartCommandProducer,
            IQueueProducer<GetPipelineExecutionStatusRequest> getPipelineExecutionStatusProducer)
        {
            _logger = logger;
            _ticketService = ticketService;
            _getPipelinesRequestProducer = getPipelinesRequestProducer;
            _createInstanceProducer = createInstanceProducer;
            _pipelineStartCommandProducer = pipelineStartCommandProducer;
            _getPipelineExecutionStatusProducer = getPipelineExecutionStatusProducer;
        }

        public Guid CreatePipelineExecution(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new CreatePipelineExecutionRequest()
            {
                TicketId = ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),

                OrganizationId = organizationId,
                RepositoryId = repositoryId,
                PipelineId = pipelineId
            };

            _createInstanceProducer.PublishMessage(message);
            _logger.LogDebug("CreatePipelineExecutionRequest Enqueued");

            return ticketId;
        }

        public Guid GetExecutionStatus(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new GetPipelineExecutionStatusRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                ExecutionId = executionId
            };

            _getPipelineExecutionStatusProducer.PublishMessage(message);

            _logger.LogDebug("GetPipelineExecutionStatus Enqueued");

            return ticketId;
        }

        public Guid GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

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

        public Guid PostStartCommand(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new PipelineStartCommandRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                RepositoryId = repositoryId,
                PipelineId = pipelineId,
                ExecutionId = executionId
            };

            _pipelineStartCommandProducer.PublishMessage(message);

            _logger.LogDebug("PipelineStartCommandRequest Enqueued");

            return ticketId;
        }
    }
}
