using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;
using System.Collections.Generic;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetEntriesFromOrgConsumer : IQueueConsumer<GetEntriesFromOrgMessage>
    {
        private IPeerService _peerService;
        private IRepositoryService _repositoryService;
        private IResourceService _resourceService;
        private IPipelineService _pipelineService;
        private IQueueProducer<GetEntriesFromOrgResult> _resultProducer;

        public GetEntriesFromOrgConsumer(IPeerService peerService, IRepositoryService repositoryService,
            IResourceService resourceService, IPipelineService pipelineService, IQueueProducer<GetEntriesFromOrgResult> resultProducer)
        {
            _peerService = peerService;
            _repositoryService = repositoryService;
            _resourceService = resourceService;
            _pipelineService = pipelineService;
            _resultProducer = resultProducer;
        }

        public async Task ConsumeAsync(GetEntriesFromOrgMessage message)
        {
            var peer = await _peerService.GetPeer(message.OrganizationId);
            var repositories = await _peerService.GetRepositoriesOfOrganization(message.OrganizationId);
            var resources = new List<Models.Resource>();
            var pipelines = new List<Models.Pipeline>();

            foreach (var repository in repositories)
            {
                resources.AddRange(await _repositoryService.GetResourcesOfRepository(repository.PeerId, repository.Id));
                pipelines.AddRange(await _repositoryService.GetPipelinesOfRepository(repository.PeerId, repository.Id));
            }

            var peerDTO = new OrganizationDTO()
            {
                Domain = peer.Domain,
                Id = peer.Id,
                Name = peer.Name,
            };

            var repositoriesDTO = new List<RepositoryDTO>();
            var resourcesDTO = new List<ResourceDTO>();
            var pipelinesDTO = new List<PipelineDTO>();


            foreach(var repository in repositories)
            {
                var repositoryDTO = new RepositoryDTO()
                {
                    Id = repository.Id,
                    Name = repository.Name,
                    OrganizationId = repository.PeerId
                };

                repositoriesDTO.Add(repositoryDTO);
            }

            foreach (var resource in resources)
            {
                var resourceDTO = new ResourceDTO()
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    OrganizationId = resource.PeerId,
                    Type = resource.ResourceType,
                    RepositoryId = resource.RepositoryId,
                };

                resourcesDTO.Add(resourceDTO);
            }

            foreach (var pipeline in pipelines)
            {
                var pipelineDTO = new PipelineDTO()
                {
                    Id = pipeline.Id,
                    Name = pipeline.Name,
                    OrganizationId = pipeline.PeerId,
                    RepositoryId = pipeline.RepositoryId,
                };

                pipelinesDTO.Add(pipelineDTO);
            }


            var resultMessage = new GetEntriesFromOrgResult()
            {
                TicketId = message.TicketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Organization = peerDTO,
                Repositories = repositoriesDTO,
                Resources = resourcesDTO,
                Pipelines = pipelinesDTO

            };

            _resultProducer.PublishMessage(resultMessage);

            return;

        }
    }
}
