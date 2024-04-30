using DAPM.ResourceRegistryMS.Api.Models.DTOs;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetOrganizationByIdConsumer : IQueueConsumer<GetOrganizationByIdMessage>
    {
        private ILogger<GetOrganizationByIdConsumer> _logger;
        private IQueueProducer<GetOrganizationsResultMessage> _getOrganizationsResultQueueProducer;
        private IPeerService _peerService;
        public GetOrganizationByIdConsumer(ILogger<GetOrganizationByIdConsumer> logger, 
            IQueueProducer<GetOrganizationsResultMessage> getOrganisationsResultQueueProducer,
            IPeerService peerService)
        {
            _logger = logger;
            _getOrganizationsResultQueueProducer = getOrganisationsResultQueueProducer;
            _peerService = peerService;
        }

        public async Task ConsumeAsync(GetOrganizationByIdMessage message)
        {
            _logger.LogInformation("Get OrganizationsByIdMessage received");

            var peer = await _peerService.GetPeer(message.OrganizationId);
            IEnumerable<OrganizationDTO> organizations = Enumerable.Empty<OrganizationDTO>();

            if(peer != null) 
            {
                var organizationDto = new OrganizationDTO
                {
                    Id = peer.Id,
                    Name = peer.Name,
                    ApiUrl = peer.ApiUrl,
                };

                organizations = organizations.Append(organizationDto);
            }

            var resultMessage = new GetOrganizationsResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Organizations = organizations
            };

            _getOrganizationsResultQueueProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
