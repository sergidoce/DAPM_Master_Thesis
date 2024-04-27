using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetResourceByIdConsumer : IQueueConsumer<GetResourceByIdMessage>
    {
        private ILogger<GetResourceByIdConsumer> _logger;
        private IQueueProducer<GetResourcesResultMessage> _getResourcesResultQueueProducer;
        public GetResourceByIdConsumer(ILogger<GetResourceByIdConsumer> logger, IQueueProducer<GetResourcesResultMessage> getResourcesResultQueueProducer)
        {
            _logger = logger;
            _getResourcesResultQueueProducer = getResourcesResultQueueProducer;
        }
        public Task ConsumeAsync(GetResourceByIdMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
