using DAPM.Orchestrator.Services;
using DAPM.Orchestrator.Services.Models;
using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes.PipelineActions
{
    public class TransferDataActionProcess : OrchestratorProcess
    {
        private Guid _executionId;
        private Guid _stepId;

        private int _sourceStorageMode;
        private int _destinationStorageMode;

        private Guid _organizationId;
        private Guid? _repositoryId;
        private Guid _resourceId;

        private Guid _destinationOrganizationId;
        private Guid? _destinationRepositoryId;

         
        private FileDTO _resourceFile;
        private ResourceDTO _resource;

        private IEnumerable<OrganizationDTO> _organizationsInRegistry;

        private string? _destinationName;

        private IdentityDTO _orchestratorIdentity;

        private Guid? _senderProcessId;

        private ResourceDTO _createdResource;
        private Dictionary<Guid, bool> _isRegistryUpdateCompleted;
        private int _registryUpdatesNotCompletedCounter;

        public TransferDataActionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid processId,
            Guid? senderProcessId, TransferDataActionDTO data, IdentityDTO orchestratorIdentity) 
            : base(engine, serviceProvider, processId)
        {
            _executionId = data.ExecutionId;
            _stepId = data.StepId;

            _organizationId = data.OriginOrganizationId;
            _repositoryId = data.OriginRepositoryId;
            _resourceId = data.OriginResourceId;

            _sourceStorageMode = data.SourceStorageMode;
            _destinationStorageMode = data.DestinationStorageMode;

            _destinationOrganizationId = data.DestinationOrganizationId;
            _destinationRepositoryId = data.DestinationRepositoryId;

            _destinationName = data.DestinationName;

            _orchestratorIdentity = orchestratorIdentity;

            _senderProcessId = senderProcessId;

            _registryUpdatesNotCompletedCounter = 0;
            _isRegistryUpdateCompleted = new Dictionary<Guid, bool>();

            _organizationsInRegistry = Enumerable.Empty<OrganizationDTO>();
    }


        public override void StartProcess()
        {
            var getOrganizationsProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOrganizationsMessage>>();

            var getOrganizationsMessage = new GetOrganizationsMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = null
            };

            getOrganizationsProducer.PublishMessage(getOrganizationsMessage);

            if (_sourceStorageMode == 1)
            {
                RetrieveResourceFromOperatorMs();
            }
            else if(_sourceStorageMode == 0)
            {
                RetrieveResourceFromRepositoryMs();
            }

        }

        public override void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {
            _organizationsInRegistry = message.Organizations;
        }

        #region Resource retrieval from operator

        private void RetrieveResourceFromOperatorMs()
        {
            var getResourceFilesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetExecutionOutputMessage>>();

            var message = new GetExecutionOutputMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                PipelineExecutionId = _executionId,
                ResourceId = _resourceId
            };

            getResourceFilesProducer.PublishMessage(message);
        }

        public override void OnGetResourceFilesFromOperatorResult(GetExecutionOutputResultMessage message) 
        {
            _resource = message.OutputResource;
            _resourceFile = message.OutputResource.File;

            if (_destinationStorageMode == 1)
            {
                SendResourceToOperatorMs();
            }
            else if (_destinationStorageMode == 0)
            {
                SendResourceToRepositoryMs();
            }
        }

        #endregion


        #region Resource retrieval from repository
        private void RetrieveResourceFromRepositoryMs()
        {
            var getResourceFilesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetResourceFilesFromRepoMessage>>();

            var message = new GetResourceFilesFromRepoMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                RepositoryId = (Guid)_repositoryId,
                ResourceId = _resourceId
            };

            getResourceFilesProducer.PublishMessage(message);

        }

        public override void OnGetResourceFilesFromRepoResult(GetResourceFilesFromRepoResultMessage message)
        {
            _resource = message.Resource;
            _resourceFile = message.Resource.File;

            if(_destinationStorageMode == 1)
            {
                SendResourceToOperatorMs();
            }
            else if(_destinationStorageMode == 0)
            {
                SendResourceToRepositoryMs();
            }
        }

        #endregion


        #region Resource sending to operator
        private void SendResourceToOperatorMs()
        {
            if(_destinationOrganizationId != _localPeerIdentity.Id)
            {
                SendResourceToPeer();
            }
            else
            {
                var postResourceToOperatorMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostInputResourceMessage>>();

                var message = new PostInputResourceMessage()
                {
                    ProcessId = _processId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    PipelineExecutionId = _executionId,
                    Resource =  _resource,
                };

                postResourceToOperatorMessageProducer.PublishMessage(message);
            }

        }

        public override void OnPostResourceToOperatorResult(PostInputResourceResultMessage message)
        {
            SendActionResult();
        }


        #endregion

        #region Resource sending to repository
        private void SendResourceToRepositoryMs()
        {
            if (_destinationOrganizationId != _localPeerIdentity.Id)
            {
                _resource.RepositoryId = _destinationRepositoryId;
                if (_destinationName != null)
                {
                    _resource.Name = _destinationName;
                }
                _resource.Type = "pipeline result";
                _resource.OrganizationId = _destinationOrganizationId;
                
                SendResourceToPeer();
            }
            else
            {
                var postResourceToRepoMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostResourceToRepoMessage>>();

                var name = string.Empty;
                if(_destinationName != null)
                {
                    name = _destinationName;
                }
                else
                    name = _resource.Name;

                var message = new PostResourceToRepoMessage()
                {
                    ProcessId = _processId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    OrganizationId = _organizationId,
                    RepositoryId = (Guid)_destinationRepositoryId,
                    Name = name,
                    ResourceType = "pipeline result",
                    File = _resourceFile,
                };

                postResourceToRepoMessageProducer.PublishMessage(message);
            }
            
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


            while (!_organizationsInRegistry.Any())
            {
                continue;
            } ;

            var targetOrganizations = _organizationsInRegistry;
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

        private void FinishProcess()
        {
            SendActionResult();
        }

        #endregion




        private void SendResourceToPeer()
        {

            while (!_organizationsInRegistry.Any())
            {
                continue;
            };

            var organization = _organizationsInRegistry.First(o => o.Id == _destinationOrganizationId);

            var sendResourceToPeerMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendResourceToPeerMessage>>();

            var identityDTO = new IdentityDTO()
            {
                Name = _localPeerIdentity.Name,
                Id = _localPeerIdentity.Id,
                Domain = _localPeerIdentity.Domain,
            };

            var sendResourceMessage = new SendResourceToPeerMessage()
            {
                SenderProcessId = _processId,
                ExecutionId = _executionId,
                TimeToLive = TimeSpan.FromMinutes(1),
                SenderPeerIdentity = identityDTO,
                TargetPeerDomain = organization.Domain,
                StorageMode = _destinationStorageMode,
                RepositoryId = _destinationRepositoryId,
                Resource = _resource,
            };

            sendResourceToPeerMessageProducer.PublishMessage(sendResourceMessage);
        }

        public override void OnSendResourceToPeerResult(SendResourceToPeerResultMessage message)
        {
            SendActionResult();
        }


        private void SendActionResult()
        {
            var actionResultDto = new ActionResultDTO()
            {
                ActionResult = ActionResult.Completed,
                ExecutionId = _executionId,
                StepId = _stepId,
                Message = "Step completed"
            };

            if (_localPeerIdentity.Id != _orchestratorIdentity.Id)
            {
                var sendActionResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendActionResultMessage>>();

                var message = new SendActionResultMessage()
                {
                    SenderProcessId = (Guid)_senderProcessId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    TargetPeerDomain = _orchestratorIdentity.Domain,
                    ActionResult = actionResultDto
                };

                sendActionResultProducer.PublishMessage(message);
            }
            else
            {
                var actionResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ActionResultMessage>>();

                var message = new ActionResultMessage()
                {
                    TimeToLive = TimeSpan.FromMinutes(1),
                    ActionResult = actionResultDto
                };

                actionResultProducer.PublishMessage(message);
            }


            EndProcess();
        }
    }
}
