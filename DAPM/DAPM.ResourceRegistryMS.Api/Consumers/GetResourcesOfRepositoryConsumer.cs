using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetResourcesOfRepositoryConsumer : IQueueConsumer<GetResourcesOfRepositoryMessage>
    {
        private ILogger<GetResourcesOfRepositoryConsumer> _logger;
        private IQueueProducer<GetResourcesResultMessage> _getResourcesResultQueueProducer;
        public GetResourcesOfRepositoryConsumer(ILogger<GetResourcesOfRepositoryConsumer> logger, IQueueProducer<GetResourcesResultMessage> getResourcesResultQueueProducer)
        {
            _logger = logger;
            _getResourcesResultQueueProducer = getResourcesResultQueueProducer;
        }

        public Task ConsumeAsync(GetResourcesOfRepositoryMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
