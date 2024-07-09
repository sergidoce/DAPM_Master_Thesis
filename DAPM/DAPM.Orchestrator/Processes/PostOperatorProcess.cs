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
    public class PostOperatorProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid _repositoryId;
        private string _name;
        private string _resourceType;

        //Resource Files
        private FileDTO _sourceCodeFile;
        private FileDTO _dockerfileFile;


        private ResourceDTO? _createdOperator;

        private Dictionary<Guid, bool> _isRegistryUpdateCompleted;
        private int _registryUpdatesNotCompletedCounter;

        private Guid _ticketId;

        public PostOperatorProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid processId,
            Guid organizationId, Guid repositoryId, string name, string resourceType, FileDTO sourceCodeFile, FileDTO dockerfileFile)
            : base(engine, serviceProvider, processId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _name = name;
            _sourceCodeFile = sourceCodeFile;
            _dockerfileFile = dockerfileFile;
            _resourceType = resourceType;

            _registryUpdatesNotCompletedCounter = 0;
            _isRegistryUpdateCompleted = new Dictionary<Guid, bool>();

            _ticketId = ticketId;

    }

        public override void StartProcess()
        {
            var postOperatorToRepoMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostOperatorToRepoMessage>>();

            var message = new PostOperatorToRepoMessage()
            {
                ProcessId = _processId,
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
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Resource = message.Resource
            };

            postResourceToRegistryMessageProducer.PublishMessage(postResourceToRegistryMessage);
        }

        public override void OnPostResourceToRegistryResult(PostResourceToRegistryResultMessage message)
        {
            _createdOperator = message.Resource;

            var getOrganizationsProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOrganizationsMessage>>();

            var getOrganizationsMessage = new GetOrganizationsMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = null
            };

            getOrganizationsProducer.PublishMessage(getOrganizationsMessage);
        }

        public override void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {

            var targetOrganizations = message.Organizations;
            var operatorsList = new List<ResourceDTO>() { _createdOperator };


            SendRegistryUpdates(targetOrganizations,
                Enumerable.Empty<OrganizationDTO>(),
                Enumerable.Empty<RepositoryDTO>(),
                operatorsList,
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
                    SenderProcessId = _processId,
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

            if (_registryUpdatesNotCompletedCounter == 0)
            {
                FinishProcess();
            }
        }


        private void FinishProcess()
        {
            var postItemProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostItemProcessResult>>();

            var itemsIds = new ItemIds()
            {
                OrganizationId = _createdOperator.OrganizationId,
                RepositoryId = (Guid)_createdOperator.RepositoryId,
                ResourceId = _createdOperator.Id,
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
