using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class ApplyRegistryUpdateConsumer : IQueueConsumer<ApplyRegistryUpdateMessage>
    {
        private IPeerService _peerService;
        private IRepositoryService _repositoryService;
        private IResourceService _resourceService;
        private IPipelineService _pipelineService;
        private IQueueProducer<ApplyRegistryUpdateResult> _resultProducer;

        public ApplyRegistryUpdateConsumer(IPeerService peerService, IRepositoryService repositoryService,
            IResourceService resourceService, IPipelineService pipelineService, IQueueProducer<ApplyRegistryUpdateResult> resultProducer)
        {
            _peerService = peerService;
            _repositoryService = repositoryService;
            _resourceService = resourceService;
            _pipelineService = pipelineService;
            _resultProducer = resultProducer;
        }

        public async Task ConsumeAsync(ApplyRegistryUpdateMessage message)
        {
            var peers = message.RegistryUpdate.Organizations;
            var repositories = message.RegistryUpdate.Repositories;
            var resources = message.RegistryUpdate.Resources;
            var pipelines = message.RegistryUpdate.Pipelines;


            foreach(var peer in peers)
            {
                await _peerService.PostPeer(peer);
            }

            foreach(var repository in repositories)
            {
                await _peerService.PostRepositoryToOrganization(repository.OrganizationId, repository);
            }

            foreach (var resource in resources)
            {
                await _resourceService.AddResource(resource);
            }

            foreach (var pipeline in pipelines)
            {
                await _repositoryService.AddPipelineToRepository(pipeline.OrganizationId, pipeline.RepositoryId, pipeline);
            }

            var resultMessage = new ApplyRegistryUpdateResult()
            {
                ProcessId = message.ProcessId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Succeeded = true,
                Message = "Succeeded",

            };

            _resultProducer.PublishMessage(resultMessage);

            return;

        }
    }
}
