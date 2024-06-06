using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.ClientApi.Services
{
    public class SystemService : ISystemService
    {
        private ITicketService _ticketService;
        private ILogger<SystemService> _logger;
        private IQueueProducer<RegisterPeerRequest> _registerPeerRequestProducer;

        public SystemService(ITicketService ticketService,
            IQueueProducer<RegisterPeerRequest> registerPeerRequestProducer,
            ILogger<SystemService> logger)
        {
            _ticketService = ticketService;
            _registerPeerRequestProducer = registerPeerRequestProducer;
            _logger = logger;
        }

        public Guid RegisterPeer(string peerName, string introductionPeerAddress, string localPeerAddress)
        {
            var ticketId = _ticketService.CreateNewTicket();

            var message = new RegisterPeerRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                IntroductionPeerAddress = introductionPeerAddress,
                LocalPeerAddress = localPeerAddress,
                PeerName = peerName,
            };

            _registerPeerRequestProducer.PublishMessage(message);

            _logger.LogDebug("RegisterPeerRequest Enqueued");

            return ticketId;
        }
    }
}
