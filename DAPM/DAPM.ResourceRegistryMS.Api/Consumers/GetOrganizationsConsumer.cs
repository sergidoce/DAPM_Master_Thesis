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
        public GetOrganizationsConsumer(ILogger<GetOrganizationsConsumer> logger, IQueueProducer<GetOrganizationsResultMessage> getOrganisationsResultQueueProducer)
        {
            _logger = logger;
            _getOrganisationsResultQueueProducer = getOrganisationsResultQueueProducer;
        }

        public Task ConsumeAsync(GetOrganizationsMessage message)
        {
            _logger.LogInformation("Get OrganisationsMessage received");

            var organisation1 = new OrganizationDTO
            {
                Id = 0,
                Name = "DTU",
                ApiUrl = "http.dtu"
            };

            var organisation2 = new OrganizationDTO
            {
                Id = 1,
                Name = "KU",
                ApiUrl = "http.ku"
            };

            var new_message = new GetOrganizationsResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Organizations = [organisation1, organisation2]
            };

            _getOrganisationsResultQueueProducer.PublishMessage(new_message);

            return Task.CompletedTask;
        }

    }
}
