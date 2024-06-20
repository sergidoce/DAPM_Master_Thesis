using Microsoft.Extensions.DependencyInjection;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes
{
    public class PostOperatorProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid _repositoryId;
        private string _name;
        private string _resourceType;

        //Resource Files
        private FileDTO _sourceCodeFile;
        private FileDTO _dockerfileFile;

        public PostOperatorProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid organizationId, Guid repositoryId, string name, string resourceType, FileDTO sourceCodeFile, FileDTO dockerfileFile)
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _name = name;
            _sourceCodeFile = sourceCodeFile;
            _dockerfileFile = dockerfileFile;
            _resourceType = resourceType;
        }

        public override void StartProcess()
        {
            var postOperatorToRepoMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostOperatorToRepoMessage>>();

            var message = new PostOperatorToRepoMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
                Name = _name,
                ResourceType = _resourceType,
                SourceCode = _sourceCodeFile,
                Dockerfile = _dockerfileFile
            };

            postOperatorToRepoMessageProducer.PublishMessage(message);
        }

        public override void OnPostResourceToRepoResult(PostResourceToRepoResultMessage message)
        {
            var postResourceToRegistryMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostResourceToRegistryMessage>>();

            var postResourceToRegistryMessage = new PostResourceToRegistryMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Resource = message.Resource
            };

            postResourceToRegistryMessageProducer.PublishMessage(postResourceToRegistryMessage);
        }

        public override void OnPostResourceToRegistryResult(PostResourceToRegistryResultMessage message)
        {
            var postItemProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostItemProcessResult>>();

            var itemsIds = new ItemIds()
            {
                OrganizationId = message.Resource.OrganizationId,
                RepositoryId = message.Resource.RepositoryId,
                ResourceId = message.Resource.Id
            };

            var postItemProcessResultMessage = new PostItemProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ItemIds = itemsIds,
                ItemType = "Operator Resource",
                Message = "The item was posted successfully",
                Succeeded = true
            };

            postItemProcessResultProducer.PublishMessage(postItemProcessResultMessage);

            EndProcess();
        }
    }
}
