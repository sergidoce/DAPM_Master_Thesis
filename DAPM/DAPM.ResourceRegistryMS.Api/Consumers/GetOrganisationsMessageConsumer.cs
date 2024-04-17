using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetOrganisationsMessageConsumer : IQueueConsumer<GetOrganisationsMessage>
    {
        private ILogger<GetOrganisationsMessageConsumer> _logger;
        private IQueueProducer<GetOrganisationsResultMessage> _getOrganisationsResultQueueProducer;
        public GetOrganisationsMessageConsumer(ILogger<GetOrganisationsMessageConsumer> logger, IQueueProducer<GetOrganisationsResultMessage> getOrganisationsResultQueueProducer)
        {
            _logger = logger;
            _getOrganisationsResultQueueProducer = getOrganisationsResultQueueProducer;
        }

        public Task ConsumeAsync(GetOrganisationsMessage message)
        {
            _logger.LogInformation("Get OrganisationsMessage received");

            var organisation1 = new Organisation
            {
                Id = "0",
                Name = "DTU",
                ApiUrl = "http.dtu"
            };

            var organisation2 = new Organisation
            {
                Id = "1",
                Name = "KU",
                ApiUrl = "http.ku"
            };

            var new_message = new GetOrganisationsResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Organisations = [organisation1, organisation2]
            };

            _getOrganisationsResultQueueProducer.PublishMessage(new_message);

            return Task.CompletedTask;
        }

    }
}
