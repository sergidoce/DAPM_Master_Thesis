﻿using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes
{
    public class PostPipelineProcess : OrchestratorProcess
    {

        private int _organizationId;
        private int _repositoryId;
        private string _pipelineName;
        private Pipeline _pipeline;

        public PostPipelineProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId
            , int organizationId, int repositoryId, Pipeline pipeline, string name)
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _pipeline = pipeline;
            _pipelineName = name;
        }   
        public override void StartProcess()
        {
            var postPipelineToRepoProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostPipelineToRepoMessage>>();

            var message = new PostPipelineToRepoMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
                Name = _pipelineName,
                Pipeline = _pipeline,
            };

            postPipelineToRepoProducer.PublishMessage(message);
        }

        public override void OnPostPipelineToRepoResult(PostPipelineToRepoResultMessage message)
        {
            var postItemProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostItemProcessResult>>();

            var postItemProcessResultMessage = new PostItemProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ItemId = message.Pipeline.Id,
                ItemType = "Pipeline",
                Message = "The item was posted successfully",
                Succeeded = true
            };

            postItemProcessResultProducer.PublishMessage(postItemProcessResultMessage);

            EndProcess();

        }

    }
}
