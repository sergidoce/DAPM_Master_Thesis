using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class PostResourceToRegistryConsumer : IQueueConsumer<PostResourceToRegistryMessage>
    {
        private ILogger<PostResourceToRegistryConsumer> _logger;
        private IResourceService _resourceService;
        private IQueueProducer<PostResourceToRegistryResultMessage> _postResourceToRegistryResultProducer;
        public PostResourceToRegistryConsumer(ILogger<PostResourceToRegistryConsumer> logger,
            IQueueProducer<PostResourceToRegistryResultMessage> postResourceToRegistryResultProducer,
            IResourceService resourceService)
        {
            _logger = logger;
            _postResourceToRegistryResultProducer = postResourceToRegistryResultProducer;
            _resourceService = resourceService;
        }
        public async Task ConsumeAsync(PostResourceToRegistryMessage message)
        {
            _logger.LogInformation("PostResourceToRegistryMessage received");

            var resourceDto = message.Resource;
            if (resourceDto != null) 
            {
                var createdResource = _resourceService.AddResource(resourceDto);
                if(createdResource != null)
                {
                    var resultMessage = new PostResourceToRegistryResultMessage
                    {
                        TicketId = message.TicketId,
                        TimeToLive = TimeSpan.FromMinutes(1),
                        Message = "Item created successfully",
                        Succeeded = true,
                        Resource = resourceDto
                    };

                    _postResourceToRegistryResultProducer.PublishMessage(resultMessage);
                    _logger.LogInformation("PostResourceToRegistryResultMessage published");
                }
            }

            return;
        }
    }
}
