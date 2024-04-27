using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetOrganizationByIdConsumer : IQueueConsumer<GetOrganizationByIdMessage>
    {
        private ILogger<GetOrganizationByIdConsumer> _logger;
        private IQueueProducer<GetOrganizationsResultMessage> _getOrganizationsResultQueueProducer;
        public GetOrganizationByIdConsumer(ILogger<GetOrganizationByIdConsumer> logger, IQueueProducer<GetOrganizationsResultMessage> getOrganisationsResultQueueProducer)
        {
            _logger = logger;
            _getOrganizationsResultQueueProducer = getOrganisationsResultQueueProducer;
        }

        public Task ConsumeAsync(GetOrganizationByIdMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
