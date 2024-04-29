using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class AddRepositoryToRegistryConsumer : IQueueConsumer<AddRepositoryToRegistryMessage>
    {
        private ILogger<AddRepositoryToRegistryConsumer> _logger;
        private IQueueProducer<CreateNewItemResultMessage> _createNewItemResultQueueProducer;
        public AddRepositoryToRegistryConsumer(ILogger<AddRepositoryToRegistryConsumer> logger, IQueueProducer<CreateNewItemResultMessage> createNewItemResultQueueProducer)
        {
            _logger = logger;
            _createNewItemResultQueueProducer = createNewItemResultQueueProducer;
        }
        public Task ConsumeAsync(AddRepositoryToRegistryMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
