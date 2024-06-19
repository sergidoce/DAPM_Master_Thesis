using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class PostOperatorToRepoConsumer : IQueueConsumer<PostOperatorToRepoMessage>
    {
        private ILogger<PostOperatorToRepoConsumer> _logger;
        private IRepositoryService _repositoryService;
        IQueueProducer<PostResourceToRepoResultMessage> _postResourceToRepoResultProducer;

        public PostOperatorToRepoConsumer(ILogger<PostOperatorToRepoConsumer> logger,
            IRepositoryService repositoryService,
            IQueueProducer<PostResourceToRepoResultMessage> postResourceToRepoResultProducer)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _postResourceToRepoResultProducer = postResourceToRepoResultProducer;
        }

        public async Task ConsumeAsync(PostOperatorToRepoMessage message)
        {
            _logger.LogInformation("PostOperatorToRepoMessage received");

            var op = await _repositoryService.CreateNewOperator(message.RepositoryId, message.Name, message.ResourceType, message.SourceCode, message.Dockerfile);

            if (op != null)
            {
                var resourceDto = new ResourceDTO
                {
                    Id = op.Id,
                    Name = message.Name,
                    OrganizationId = message.OrganizationId,
                    RepositoryId = message.RepositoryId,
                    Type = message.ResourceType,
                };

                var postResourceToRepoResult = new PostResourceToRepoResultMessage
                {
                    TicketId = message.TicketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Resource = resourceDto
                };

                _postResourceToRepoResultProducer.PublishMessage(postResourceToRepoResult);

                _logger.LogInformation("PostOperatorToRepoResultMessage produced");

            }
            else
            {
                _logger.LogInformation("Creation of new operator failed");
            }

            return;
        }
    }
}
