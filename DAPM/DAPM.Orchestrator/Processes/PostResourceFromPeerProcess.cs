using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Operator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes
{
    public class PostResourceFromPeerProcess : OrchestratorProcess
    {

        private ResourceDTO _resource;
        private ResourceDTO _createdResource;

        private IdentityDTO _senderIdentity;

        private Dictionary<Guid, bool> _isRegistryUpdateCompleted;
        private int _registryUpdatesNotCompletedCounter;

        private int _storageMode;
        private Guid _executionId;


        public PostResourceFromPeerProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
             Guid processId, ResourceDTO resource, int storageMode, Guid executionId, IdentityDTO senderIdentity)
            : base(engine, serviceProvider, processId)
        {
            
            _resource = resource;
            _executionId = executionId;
            _storageMode = storageMode;
            _senderIdentity = senderIdentity;
            _registryUpdatesNotCompletedCounter = 0;
            _isRegistryUpdateCompleted = new Dictionary<Guid, bool>();
        }

        public override void StartProcess()
        {

            if(_storageMode == 0)
            {
                PostResourceToRepository();
            }
            else
            {
                PostResourceToOperator();
            }
        }

        #region To Repository
        private void PostResourceToRepository()
        {
            var postResourceToRepoMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostResourceToRepoMessage>>();

            var message = new PostResourceToRepoMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _resource.OrganizationId,
                RepositoryId = (Guid)_resource.RepositoryId,
                Name = _resource.Name,
                ResourceType = _resource.Type,
                File = _resource.File
            };

            postResourceToRepoMessageProducer.PublishMessage(message);
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
            _createdResource = message.Resource;

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
            var resourcesList = new List<ResourceDTO>() { _createdResource };


            SendRegistryUpdates(targetOrganizations,
                Enumerable.Empty<OrganizationDTO>(),
                Enumerable.Empty<RepositoryDTO>(),
                resourcesList,
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

        #endregion

        #region To Operator

        private void PostResourceToOperator()
        {
            var postResourceToOperatorMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostInputResourceMessage>>();

            var message = new PostInputResourceMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                PipelineExecutionId = _executionId,
                Resource = _resource,
            };

            postResourceToOperatorMessageProducer.PublishMessage(message);
        }

        public override void OnPostResourceToOperatorResult(PostInputResourceResultMessage message)
        {
            FinishProcess();
        }

        #endregion


        private void FinishProcess()
        {
            var postResourceFromPeerResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostResourceFromPeerResultMessage>>();

            var postResourceFromPeerResultMessage = new PostResourceFromPeerResultMessage()
            {
                SenderProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                TargetPeerDomain = _senderIdentity.Domain,
                Succeeded = true
            };

            postResourceFromPeerResultProducer.PublishMessage(postResourceFromPeerResultMessage);

            EndProcess();
        }
    }
}
