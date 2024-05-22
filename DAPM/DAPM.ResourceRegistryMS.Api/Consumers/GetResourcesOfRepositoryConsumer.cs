using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetResourcesOfRepositoryConsumer : IQueueConsumer<GetResourcesOfRepositoryMessage>
    {
        private ILogger<GetResourcesOfRepositoryConsumer> _logger;
        private IQueueProducer<GetResourcesResultMessage> _getResourcesResultQueueProducer;
        private IRepositoryService _repositoryService;
        public GetResourcesOfRepositoryConsumer(ILogger<GetResourcesOfRepositoryConsumer> logger,
            IQueueProducer<GetResourcesResultMessage> getResourcesResultQueueProducer,
            IRepositoryService repositoryService)
        {
            _logger = logger;
            _getResourcesResultQueueProducer = getResourcesResultQueueProducer;
            _repositoryService = repositoryService;
        }

        public async Task ConsumeAsync(GetResourcesOfRepositoryMessage message)
        {
            _logger.LogInformation("GetResourcesOfRepositoryMessage received");

            var resources = await _repositoryService.GetResourcesOfRepository(message.OrganizationId, message.RepositoryId);
            IEnumerable<ResourceDTO> resourcesDTOs = Enumerable.Empty<ResourceDTO>();

            foreach (var resource in resources)
            {
                var r = new ResourceDTO
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    OrganizationId = resource.PeerId,
                    RepositoryId = resource.RepositoryId,
                    Type = "EventLog",
                    Extension = ".csv"
                };

                resourcesDTOs = resourcesDTOs.Append(r);
            }

            var resultMessage = new GetResourcesResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Resources = resourcesDTOs
            };

            _getResourcesResultQueueProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
