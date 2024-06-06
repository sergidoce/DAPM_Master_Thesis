using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetRepositoriesConsumer : IQueueConsumer<GetRepositoriesMessage>
    {
        private ILogger<GetRepositoriesConsumer> _logger;
        private IQueueProducer<GetRepositoriesResultMessage> _getRepositoriesResultQueueProducer;
        private IPeerService _peerService;
        private IRepositoryService _repositoryService;

        public GetRepositoriesConsumer(
            ILogger<GetRepositoriesConsumer> logger,
            IQueueProducer<GetRepositoriesResultMessage> getRepositoriesResultQueueProducer,
            IPeerService peerService,
            IRepositoryService repositoryService)
        {
            _logger = logger;
            _getRepositoriesResultQueueProducer = getRepositoriesResultQueueProducer;
            _peerService = peerService;
            _repositoryService = repositoryService;
        }

        public async Task ConsumeAsync(GetRepositoriesMessage message)
        {
            _logger.LogInformation("GetRepositoriesMessage received");

            var repositories = Enumerable.Empty<Repository>();

            if (message.RepositoryId != null)
            {
                var repository = await _repositoryService.GetRepositoryById(message.OrganizationId, (Guid)message.RepositoryId);
                repositories = repositories.Append(repository);
            }
            else
            {
                repositories = await _peerService.GetRepositoriesOfOrganization(message.OrganizationId);
            }

            IEnumerable<RepositoryDTO> repositoriesDTOs = Enumerable.Empty<RepositoryDTO>();

            foreach (var repository in repositories)
            {
                var repo = new RepositoryDTO
                {
                    Id = repository.Id,
                    Name = repository.Name,
                    OrganizationId = repository.PeerId,
                };

                repositoriesDTOs = repositoriesDTOs.Append(repo);
            }

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
