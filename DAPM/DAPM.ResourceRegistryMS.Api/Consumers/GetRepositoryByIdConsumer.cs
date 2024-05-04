using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetRepositoryByIdConsumer : IQueueConsumer<GetRepositoryByIdMessage>
    {
        private ILogger<GetRepositoryByIdConsumer> _logger;
        private IQueueProducer<GetRepositoriesResultMessage> _getRepositoriesResultQueueProducer;
        private IRepositoryService _repositoryService;
        public GetRepositoryByIdConsumer(ILogger<GetRepositoryByIdConsumer> logger,
            IQueueProducer<GetRepositoriesResultMessage> getRepositoriesResultQueueProducer,
            IRepositoryService repositoryService)
        {
            _logger = logger;
            _getRepositoriesResultQueueProducer = getRepositoriesResultQueueProducer;
            _repositoryService = repositoryService;
        }
        public async Task ConsumeAsync(GetRepositoryByIdMessage message)
        {
            _logger.LogInformation("GetRepositoryByIdMessage received");

            var repository = await _repositoryService.GetRepositoryById(message.OrganizationId, message.RepositoryId);
            IEnumerable<RepositoryDTO> repositoriesDTOs = Enumerable.Empty<RepositoryDTO>();

            var repo = new RepositoryDTO
            {
                Id = repository.Id,
                Name = repository.Name,
                OrganizationId = repository.PeerId,
            };

            repositoriesDTOs = repositoriesDTOs.Append(repo);
            

            var resultMessage = new GetRepositoriesResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Repositories = repositoriesDTOs
            };

            _getRepositoriesResultQueueProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
