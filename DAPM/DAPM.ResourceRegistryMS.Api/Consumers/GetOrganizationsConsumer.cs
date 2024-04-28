using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetOrganizationsConsumer : IQueueConsumer<GetOrganizationsMessage>
    {
        private ILogger<GetOrganizationsConsumer> _logger;
        private IQueueProducer<GetOrganizationsResultMessage> _getOrganisationsResultQueueProducer;
        private IPeerService _peerService;

        public GetOrganizationsConsumer(
            ILogger<GetOrganizationsConsumer> logger, 
            IQueueProducer<GetOrganizationsResultMessage> getOrganisationsResultQueueProducer,
            IPeerService peerService)
        {
            _logger = logger;
            _getOrganisationsResultQueueProducer = getOrganisationsResultQueueProducer;
            _peerService = peerService;
        }

        public async Task ConsumeAsync(GetOrganizationsMessage message)
        {
            _logger.LogInformation("Get OrganisationsMessage received");

            var peers = await _peerService.GetAllPeers();
            IEnumerable<OrganizationDTO> organizations = Enumerable.Empty<OrganizationDTO>();

            foreach (var peer in peers)
            {
                var org = new OrganizationDTO
                {
                    Id = peer.Id,
                    Name = peer.Name,
                    ApiUrl = peer.ApiUrl,
                };

                organizations.Append(org);
            }

            var resultMessage = new GetOrganizationsResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Organizations = organizations
            };

            _getOrganisationsResultQueueProducer.PublishMessage(resultMessage);

            return;
        }

    }
}
