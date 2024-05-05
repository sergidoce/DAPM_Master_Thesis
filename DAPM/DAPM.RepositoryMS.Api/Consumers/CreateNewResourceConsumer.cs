using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class CreateNewResourceConsumer : IQueueConsumer<CreateNewResourceMessage>
    {

        private ILogger<CreateNewResourceConsumer> _logger;
        private IRepositoryService _repositoryService;
        IQueueProducer<AddResourceToRegistryMessage> _addResourceToRegistryProducer;

        public CreateNewResourceConsumer(ILogger<CreateNewResourceConsumer> logger, 
            IRepositoryService repositoryService,
            IQueueProducer<AddResourceToRegistryMessage> addResourceToRegistryProducer)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _addResourceToRegistryProducer = addResourceToRegistryProducer;
        }

        public async Task ConsumeAsync(CreateNewResourceMessage message)
        {
            _logger.LogInformation("CreateNewResourceMessage received");

            int resourceId = await _repositoryService.CreateNewResource(message.RepositoryId, message.Name, message.ResourceFile);

            if (resourceId != -1)
            {
                var resourceDto = new ResourceDTO
                {
                    Id = resourceId,
                    Name = message.Name,
                    OrganizationId = message.OrganizationId,
                    RepositoryId = message.RepositoryId,
                    Type = "EventLog",
                    Extension = ".csv"
                };

                var addResourceToRegistryMessage = new AddResourceToRegistryMessage
                {
                    TicketId = message.TicketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Resource = resourceDto
                };

                _addResourceToRegistryProducer.PublishMessage(addResourceToRegistryMessage);

                _logger.LogInformation("AddResourceToRegistryMessage produced");

            }
            else
            {
                _logger.LogInformation("Creation of new resource failed");
            }

            return;
        }
    }
}
