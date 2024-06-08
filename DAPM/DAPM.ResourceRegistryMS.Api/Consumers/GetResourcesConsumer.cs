using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetResourcesConsumer : IQueueConsumer<GetResourcesMessage>
    {
        private ILogger<GetResourcesConsumer> _logger;
        private IQueueProducer<GetResourcesResultMessage> _getResourcesResultQueueProducer;
        private IRepositoryService _repositoryService;
        private IResourceService _resourceService;
        public GetResourcesConsumer(ILogger<GetResourcesConsumer> logger,
            IQueueProducer<GetResourcesResultMessage> getResourcesResultQueueProducer,
            IRepositoryService repositoryService,
            IResourceService resourceService)
        {
            _logger = logger;
            _getResourcesResultQueueProducer = getResourcesResultQueueProducer;
            _repositoryService = repositoryService;
            _resourceService = resourceService;
        }

        public async Task ConsumeAsync(GetResourcesMessage message)
        {
            _logger.LogInformation("GetResourcesMessage received");

            var resources = Enumerable.Empty<Models.Resource>();

            if (message.ResourceId != null)
            {
                var resource = await _resourceService.GetResourceById(message.OrganizationId, message.RepositoryId, (Guid)message.ResourceId);
                resources = resources.Append(resource);
            }
            else
            {
                resources = await _repositoryService.GetResourcesOfRepository(message.OrganizationId, message.RepositoryId);
            }

            IEnumerable<ResourceDTO> resourcesDTOs = Enumerable.Empty<ResourceDTO>();

            foreach (var resource in resources)
            {
                var r = new ResourceDTO
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    OrganizationId = resource.PeerId,
                    RepositoryId = resource.RepositoryId,
                    Type = resource.ResourceType,
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
