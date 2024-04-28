using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetRepositoryByIdConsumer : IQueueConsumer<GetRepositoryByIdMessage>
    {
        private ILogger<GetRepositoryByIdConsumer> _logger;
        private IQueueProducer<GetRepositoriesResultMessage> _getRepositoriesResultQueueProducer;
        public GetRepositoryByIdConsumer(ILogger<GetRepositoryByIdConsumer> logger, IQueueProducer<GetRepositoriesResultMessage> getRepositoriesResultQueueProducer)
        {
            _logger = logger;
            _getRepositoriesResultQueueProducer = getRepositoriesResultQueueProducer;
        }
        public Task ConsumeAsync(GetRepositoryByIdMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
