using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetRepositoriesOfUserConsumer : IQueueConsumer<GetRepositoriesOfUserMessage>
    {
        private ILogger<GetRepositoriesOfUserConsumer> _logger;
        private IQueueProducer<GetRepositoriesResultMessage> _getRepositoriesResultQueueProducer;
        public GetRepositoriesOfUserConsumer(ILogger<GetRepositoriesOfUserConsumer> logger, IQueueProducer<GetRepositoriesResultMessage> getRepositoriesResultQueueProducer)
        {
            _logger = logger;
            _getRepositoriesResultQueueProducer = getRepositoriesResultQueueProducer;
        }

        public Task ConsumeAsync(GetRepositoriesOfUserMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
