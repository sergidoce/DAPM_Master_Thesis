using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class PostRepoToRepoConsumer : IQueueConsumer<PostRepoToRepoMessage>
    {
        private ILogger<PostRepoToRepoConsumer> _logger;
        private IRepositoryService _repositoryService;
        IQueueProducer<PostRepoToRepoResultMessage> _postRepoToRepoResultProducer;

        public PostRepoToRepoConsumer(ILogger<PostRepoToRepoConsumer> logger, IRepositoryService repositoryService,
            IQueueProducer<PostRepoToRepoResultMessage> postRepoToRepoResultProducer) 
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _postRepoToRepoResultProducer = postRepoToRepoResultProducer;
        }
        public async Task ConsumeAsync(PostRepoToRepoMessage message)
        {
            _logger.LogInformation("PostRepoToRepoMessage received");

            Repository repository = await _repositoryService.CreateNewRepository(message.Name);

            RepositoryDTO repositoryDTO = new RepositoryDTO
            {
                Id = repository.Id,
                Name = repository.Name,
            };

            var postResourceToRepoResult = new PostRepoToRepoResultMessage
            {
                TicketId = message.TicketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Repository = repositoryDTO,
                Succeeded = true,
                Message = "The repository was created successfully"
            };

            _postRepoToRepoResultProducer.PublishMessage(postResourceToRepoResult);

            _logger.LogInformation("PostRepoToRepoResultMessage produced");


            return;
        }
    }
}
