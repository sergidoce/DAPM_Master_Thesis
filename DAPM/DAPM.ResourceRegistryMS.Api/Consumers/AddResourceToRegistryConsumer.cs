using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class AddResourceToRegistryConsumer : IQueueConsumer<AddResourceToRegistryMessage>
    {
        private ILogger<AddResourceToRegistryConsumer> _logger;
        private IResourceService _resourceService;
        private IQueueProducer<CreateNewItemResultMessage> _createNewItemResultQueueProducer;
        public AddResourceToRegistryConsumer(ILogger<AddResourceToRegistryConsumer> logger,
            IQueueProducer<CreateNewItemResultMessage> createNewItemResultQueueProducer,
            IResourceService resourceService)
        {
            _logger = logger;
            _createNewItemResultQueueProducer = createNewItemResultQueueProducer;
            _resourceService = resourceService;
        }
        public async Task ConsumeAsync(AddResourceToRegistryMessage message)
        {
            _logger.LogInformation("AddResourceToRegistryMessage received");

            var resourceDto = message.Resource;
            if (resourceDto != null) 
            {
                var createdResource = _resourceService.AddResource(resourceDto);
                if(createdResource != null)
                {
                    var resultMessage = new CreateNewItemResultMessage
                    {
                        TicketId = message.TicketId,
                        TimeToLive = TimeSpan.FromMinutes(1),
                        ItemId = createdResource.Id,
                        ItemType = "Resource",
                        Message = "Item created successfully",
                        Succeeded = true,
                    };

                    _createNewItemResultQueueProducer.PublishMessage(resultMessage);
                    _logger.LogInformation("CreateNewItemResultMessage published");
                }
            }

            return;
        }
    }
}
