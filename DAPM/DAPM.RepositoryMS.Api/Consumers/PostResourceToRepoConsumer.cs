using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class PostResourceToRepoConsumer : IQueueConsumer<PostResourceToRepoMessage>
    {

        private ILogger<PostResourceToRepoConsumer> _logger;
        private IRepositoryService _repositoryService;
        IQueueProducer<PostResourceToRepoResultMessage> _postResourceToRepoResultProducer;

        public PostResourceToRepoConsumer(ILogger<PostResourceToRepoConsumer> logger, 
            IRepositoryService repositoryService,
            IQueueProducer<PostResourceToRepoResultMessage> postResourceToRepoResultProducer)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _postResourceToRepoResultProducer = postResourceToRepoResultProducer;
        }

        public async Task ConsumeAsync(PostResourceToRepoMessage message)
        {
            _logger.LogInformation("PostResourceToRepoMessage received");

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

                var postResourceToRepoResult = new PostResourceToRepoResultMessage
                {
                    TicketId = message.TicketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Resource = resourceDto
                };

                _postResourceToRepoResultProducer.PublishMessage(postResourceToRepoResult);

                _logger.LogInformation("PostResourceToRepoResultMessage produced");

            }
            else
            {
                _logger.LogInformation("Creation of new resource failed");
            }

            return;
        }
    }
}
