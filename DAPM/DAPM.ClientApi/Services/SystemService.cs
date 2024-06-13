using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.ClientApi.Services
{
    public class SystemService : ISystemService
    {
        private ITicketService _ticketService;
        private ILogger<SystemService> _logger;
        private IQueueProducer<CollabHandshakeRequest> _collabHandshakeRequestProducer;

        public SystemService(ITicketService ticketService,
            IQueueProducer<CollabHandshakeRequest> collabHandshakeRequestProducer,
            ILogger<SystemService> logger)
        {
            _ticketService = ticketService;
            _collabHandshakeRequestProducer = collabHandshakeRequestProducer;
            _logger = logger;
        }

        public Guid StartCollabHandshake(string targetPeerDomain)
        {
            var ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new CollabHandshakeRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                RequestedPeerDomain = targetPeerDomain,
     
            };

            _collabHandshakeRequestProducer.PublishMessage(message);

            _logger.LogDebug("CollabHandshakeRequest Enqueued");

            return ticketId;
        }
    }
}
