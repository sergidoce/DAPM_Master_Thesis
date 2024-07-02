using DAPM.Orchestrator.Services;
using DAPM.Orchestrator.Services.Models;
using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
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

        private string? _destinationName;

        private IdentityDTO _orchestratorIdentity;

        public TransferDataActionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId,
            TransferDataActionDTO data, IdentityDTO orchestratorIdentity) 
            : base(engine, serviceProvider, ticketId)
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
        }


        public override void StartProcess()
        {

            if(_sourceStorageMode == 1)
            {
                RetrieveResourceFromOperatorMs();
            }
            else if(_sourceStorageMode == 0)
            {
                RetrieveResourceFromRepositoryMs();
            }

        }

        #region Resource retrieval from operator

        private void RetrieveResourceFromOperatorMs()
        {
            var getResourceFilesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetExecutionOutputMessage>>();

            var message = new GetExecutionOutputMessage()
            {
                TicketId = _ticketId,
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
                TicketId = _ticketId,
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
                    TicketId = _ticketId,
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
                    TicketId = _ticketId,
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
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Resource = message.Resource
            };

            postResourceToRegistryMessageProducer.PublishMessage(postResourceToRegistryMessage);
        }

        public override void OnPostResourceToRegistryResult(PostResourceToRegistryResultMessage message)
        {
            SendActionResult();
        }

        #endregion




        private void SendResourceToPeer()
        {
            var getOrganizationsProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOrganizationsMessage>>();


            var getOrganizationsMessage = new GetOrganizationsMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _destinationOrganizationId
            };

            getOrganizationsProducer.PublishMessage(getOrganizationsMessage);
        }

        public override void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {
            var sendResourceToPeerMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendResourceToPeerMessage>>();

            var identityDTO = new IdentityDTO()
            {
                Name = _localPeerIdentity.Name,
                Id = _localPeerIdentity.Id,
                Domain = _localPeerIdentity.Domain,
            };

            var sendResourceMessage = new SendResourceToPeerMessage()
            {
                TicketId = _stepId,
                ExecutionId = _executionId,
                TimeToLive = TimeSpan.FromMinutes(1),
                SenderPeerIdentity = identityDTO,
                TargetPeerDomain = message.Organizations.First().Domain,
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
                    TicketId = _ticketId,
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
                    TicketId = _ticketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    ActionResult = actionResultDto
                };

                actionResultProducer.PublishMessage(message);
            }


            EndProcess();
        }
    }
}
