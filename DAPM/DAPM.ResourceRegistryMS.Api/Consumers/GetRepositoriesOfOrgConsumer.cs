using DAPM.ResourceRegistryMS.Api.Services;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetRepositoriesOfOrgConsumer : IQueueConsumer<GetRepositoriesOfOrgMessage>
    {
        private ILogger<GetRepositoriesOfOrgConsumer> _logger;
        private IQueueProducer<GetRepositoriesResultMessage> _getRepositoriesResultQueueProducer;
        private IPeerService _peerService;
        public GetRepositoriesOfOrgConsumer(
            ILogger<GetRepositoriesOfOrgConsumer> logger,
            IQueueProducer<GetRepositoriesResultMessage> getRepositoriesResultQueueProducer,
            IPeerService peerService)
        {
            _logger = logger;
            _getRepositoriesResultQueueProducer = getRepositoriesResultQueueProducer;
            _peerService = peerService;
        }

        public async Task ConsumeAsync(GetRepositoriesOfOrgMessage message)
        {
            _logger.LogInformation("GetRepositoriesOfOrgMessage received");

            var repositories = await _peerService.GetRepositoriesOfOrganization(message.OrganizationId);
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
