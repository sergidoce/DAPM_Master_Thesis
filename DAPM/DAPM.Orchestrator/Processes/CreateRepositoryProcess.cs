using Microsoft.Extensions.DependencyInjection;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class CreateRepositoryProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private string _repositoryName; 

        public CreateRepositoryProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid organizationId, string name) 
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryName = name;
        }

        public override void StartProcess()
        {
            var postRepositoryToRepoProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostRepoToRepoMessage>>();

            var message = new PostRepoToRepoMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Name = _repositoryName
            };

            postRepositoryToRepoProducer.PublishMessage(message);
        }

        public override void OnCreateRepoInRepoResult(PostRepoToRepoResultMessage message)
        {
            var postRepositoryToRegistryProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostRepositoryToRegistryMessage>>();

            message.Repository.OrganizationId = _organizationId;

            var postRepoToRegistryMessage = new PostRepositoryToRegistryMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Repository = message.Repository
            };

            postRepositoryToRegistryProducer.PublishMessage(postRepoToRegistryMessage);
        }

        public override void OnPostRepoToRegistryResult(PostRepoToRegistryResultMessage message)
        {
            var postItemProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostItemProcessResult>>();

            var postItemProcessResultMessage = new PostItemProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ItemId = message.Repository.Id,
                ItemType = "Repository",
                Message = "The item was posted successfully",
                Succeeded = true
            };

            postItemProcessResultProducer.PublishMessage(postItemProcessResultMessage);

            EndProcess();
        }
    }
}
