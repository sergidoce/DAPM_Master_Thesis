using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetResourcesOfUserConsumer : IQueueConsumer<GetResourcesOfUserMessage>
    {
        private ILogger<GetResourcesOfUserConsumer> _logger;
        private IQueueProducer<GetResourcesResultMessage> _getResourcesResultQueueProducer;
        public GetResourcesOfUserConsumer(ILogger<GetResourcesOfUserConsumer> logger, IQueueProducer<GetResourcesResultMessage> getResourcesResultQueueProducer)
        {
            _logger = logger;
            _getResourcesResultQueueProducer = getResourcesResultQueueProducer;
        }

        public Task ConsumeAsync(GetResourcesOfUserMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
