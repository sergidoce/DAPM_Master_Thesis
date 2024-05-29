using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class AddRepositoryToRegistryConsumer : IQueueConsumer<AddRepositoryToRegistryMessage>
    {
        private ILogger<AddRepositoryToRegistryConsumer> _logger;
        private IQueueProducer<PostItemProcessResult> _createNewItemResultQueueProducer;
        public AddRepositoryToRegistryConsumer(ILogger<AddRepositoryToRegistryConsumer> logger, IQueueProducer<PostItemProcessResult> createNewItemResultQueueProducer)
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
