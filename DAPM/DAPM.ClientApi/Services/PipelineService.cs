using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ClientApi.Services
{
    public class PipelineService : IPipelineService
    {
        private readonly ILogger<PipelineService> _logger;
        private readonly ITicketService _ticketService;


        public PipelineService(
            ILogger<PipelineService> logger,
            ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Guid GetPipelineById(int repositoryId, int pipelineId)
        {
            //Guid ticketId = _ticketService.CreateNewTicket();

            //var message = new GetPipelineByIdMessage
            //{
            //    TimeToLive = TimeSpan.FromMinutes(1),
            //    TicketId = ticketId,
            //    OrganizationId = organizationId,
            //    RepositoryId = repositoryId
            //};

            //_getRepoByIdProducer.PublishMessage(message);

            //_logger.LogDebug("GetRepositoryByIdMessage Enqueued");

            //return ticketId;

            throw new NotImplementedException();
        }
    }
}
