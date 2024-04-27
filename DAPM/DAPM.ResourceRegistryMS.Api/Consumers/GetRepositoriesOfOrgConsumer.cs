using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetRepositoriesOfOrgConsumer : IQueueConsumer<GetRepositoriesOfOrgMessage>
    {
        private ILogger<GetRepositoriesOfOrgConsumer> _logger;
        private IQueueProducer<GetRepositoriesResultMessage> _getRepositoriesResultQueueProducer;
        public GetRepositoriesOfOrgConsumer(ILogger<GetRepositoriesOfOrgConsumer> logger, IQueueProducer<GetRepositoriesResultMessage> getRepositoriesResultQueueProducer)
        {
            _logger = logger;
            _getRepositoriesResultQueueProducer = getRepositoriesResultQueueProducer;
        }

        public Task ConsumeAsync(GetRepositoriesOfOrgMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
