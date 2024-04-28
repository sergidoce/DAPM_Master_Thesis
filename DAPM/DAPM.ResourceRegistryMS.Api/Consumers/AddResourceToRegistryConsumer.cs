using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class AddResourceToRegistryConsumer : IQueueConsumer<AddResourceToRegistryMessage>
    {
        private ILogger<AddResourceToRegistryConsumer> _logger;
        private IQueueProducer<CreateNewItemResultMessage> _createNewItemResultQueueProducer;
        public AddResourceToRegistryConsumer(ILogger<AddResourceToRegistryConsumer> logger, IQueueProducer<CreateNewItemResultMessage> createNewItemResultQueueProducer)
        {
            _logger = logger;
            _createNewItemResultQueueProducer = createNewItemResultQueueProducer;
        }
        public Task ConsumeAsync(AddResourceToRegistryMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
