using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class PostRepositoryToRegistryConsumer : IQueueConsumer<PostRepositoryToRegistryMessage>
    {
        private ILogger<PostRepositoryToRegistryConsumer> _logger;
        private IQueueProducer<PostRepoToRegistryResultMessage> _postRepoToRegistryResultMessageProducer;
        private IPeerService _peerService;

        public PostRepositoryToRegistryConsumer(ILogger<PostRepositoryToRegistryConsumer> logger,
            IQueueProducer<PostRepoToRegistryResultMessage> postRepoToRegistryResultMessageProducer,
            IPeerService peerService)
        {
            _logger = logger;
            _postRepoToRegistryResultMessageProducer = postRepoToRegistryResultMessageProducer;
            _peerService = peerService;
        }
        public async Task ConsumeAsync(PostRepositoryToRegistryMessage message)
        {
            _logger.LogInformation("PostRepositoryToRegistryMessage received");


            var createdRepository = await _peerService.PostRepositoryToOrganization(message.Repository.OrganizationId, message.Repository);
            if (createdRepository != null)
            {
                var repositoryDto = new RepositoryDTO()
                {
                    Id = createdRepository.Id,
                    Name = createdRepository.Name,
                    OrganizationId = createdRepository.PeerId,
                };

                var resultMessage = new PostRepoToRegistryResultMessage
                {
                    TicketId = message.TicketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Message = "Repository entry created successfully",
                    Succeeded = true,
                    Repository = repositoryDto
                };

                _postRepoToRegistryResultMessageProducer.PublishMessage(resultMessage);
                _logger.LogInformation("PostRepositoryToRegistryResultMessage published");
            }


            return;

        }
    }
}
