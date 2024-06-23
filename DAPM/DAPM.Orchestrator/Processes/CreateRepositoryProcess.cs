using DAPM.Orchestrator.Services;
using DAPM.Orchestrator.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes
{
    public class CreateRepositoryProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private string _repositoryName;

        private RepositoryDTO? _createdRepository;

        private Dictionary<Guid, bool> _isRegistryUpdateCompleted;
        private int _registryUpdatesNotCompletedCounter;

        public CreateRepositoryProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid organizationId, string name) 
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryName = name;
            _registryUpdatesNotCompletedCounter = 0;
            _isRegistryUpdateCompleted = new Dictionary<Guid, bool>();
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
            _createdRepository = message.Repository;

            var getOrganizationsProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOrganizationsMessage>>();

            var getOrganizationsMessage = new GetOrganizationsMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = null
            };

            getOrganizationsProducer.PublishMessage(getOrganizationsMessage);
        }

        public override void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {

            var targetOrganizations = message.Organizations;
            var repositoriesList = new List<RepositoryDTO>() { _createdRepository};

            
            SendRegistryUpdates(targetOrganizations,
                Enumerable.Empty<OrganizationDTO>(),
                repositoriesList,
                Enumerable.Empty<ResourceDTO>(),
                Enumerable.Empty<PipelineDTO>());
            

        }

        private void SendRegistryUpdates(IEnumerable<OrganizationDTO> targetOrganizations, IEnumerable<OrganizationDTO> organizations,
            IEnumerable<RepositoryDTO> repositories, IEnumerable<ResourceDTO> resources, IEnumerable<PipelineDTO> pipelines)
        {

            var sendRegistryUpdateProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendRegistryUpdateMessage>>();
            var identityDTO = new IdentityDTO()
            {
                Domain = _localPeerIdentity.Domain,
                Id = _localPeerIdentity.Id,
                Name = _localPeerIdentity.Name,
            };

            var registryUpdate = new RegistryUpdateDTO()
            {
                Organizations = organizations,
                Repositories = repositories,
                Pipelines = pipelines,
                Resources = resources,
            };


            var registryUpdateMessages = new List<SendRegistryUpdateMessage>();

            foreach (var organization in targetOrganizations)
            {

                if (organization.Id == _localPeerIdentity.Id)
                    continue;

                var domain = organization.Domain;
                var registryUpdateMessage = new SendRegistryUpdateMessage()
                {
                    TargetPeerDomain = domain,
                    SenderPeerIdentity = identityDTO,
                    TicketId = _ticketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    RegistryUpdate = registryUpdate,
                    IsPartOfHandshake = false,
                };

                registryUpdateMessages.Add(registryUpdateMessage);
                _isRegistryUpdateCompleted[organization.Id] = false;
                _registryUpdatesNotCompletedCounter++;
            }

            if (registryUpdateMessages.Count() == 0)
            {
                FinishProcess();
            }
            else
            {
                foreach (var message in registryUpdateMessages)
                    sendRegistryUpdateProducer.PublishMessage(message);
            }

        }

        public override void OnRegistryUpdateAck(RegistryUpdateAckMessage message)
        {
            var organizationId = message.PeerSenderIdentity.Id;
            if (message.RegistryUpdateAck.IsCompleted)
            {
                _isRegistryUpdateCompleted[(Guid)organizationId] = true;
                _registryUpdatesNotCompletedCounter--;
            }

            if(_registryUpdatesNotCompletedCounter == 0)
            {
                FinishProcess();
            }
        }


        private void FinishProcess()
        {
            var postItemProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostItemProcessResult>>();

            var itemsIds = new ItemIds()
            {
                OrganizationId = _createdRepository.OrganizationId,
                RepositoryId = _createdRepository.Id,
            };

            var postItemProcessResultMessage = new PostItemProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ItemIds = itemsIds,
                ItemType = "Repository",
                Message = "The item was posted successfully",
                Succeeded = true
            };

            postItemProcessResultProducer.PublishMessage(postItemProcessResultMessage);

            EndProcess();
        }


    }
}
