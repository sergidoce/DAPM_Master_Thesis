
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class PostResourceProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid _repositoryId;
        private string _name;
        private byte[] _file;

        public PostResourceProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid organizationId, Guid repositoryId, string name, byte[] resourceFile) 
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _name = name;
            _file = resourceFile;
        }

        public override void StartProcess()
        {
            var postResourceToRepoMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostResourceToRepoMessage>>();

            var message = new PostResourceToRepoMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
                Name = _name,
                ResourceFile = _file
            };

            postResourceToRepoMessageProducer.PublishMessage(message);
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
                ItemType = "Resource",
                Message = "The item was posted successfully",
                Succeeded = true
            };

            postItemProcessResultProducer.PublishMessage(postItemProcessResultMessage);

            EndProcess();
        }
    }
}
