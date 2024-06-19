
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
        private Identity _localNodeIdentity;

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

        public TransferDataActionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, TransferDataActionDTO data) 
            : base(engine, serviceProvider, ticketId)
        {
            _executionId = data.ExecutionId;
            _stepId = data.StepId;

            _organizationId = data.OriginOrganizationId;
            _repositoryId = data.OriginRepositoryId;
            _resourceId = data.OriginResourceId;

            _destinationOrganizationId = data.DestinationOrganizationId;
            _destinationRepositoryId = data.DestinationRepositoryId;


            var identityService = _serviceScope.ServiceProvider.GetRequiredService<IIdentityService>();
            _localNodeIdentity = identityService.GetIdentity();
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
            var getResourceFilesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetResourceFilesFromOperatorMessage>>();

            var message = new GetResourceFilesFromOperatorMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                StepId = _stepId,
                ExecutionId = _executionId,
                ResourceId = _resourceId
            };

            getResourceFilesProducer.PublishMessage(message);
        }

        public override void OnGetResourceFilesFromOperatorResult(GetResourceFilesFromOperatorResultMessage message) 
        {
            _resourceFile = message.Files.First();

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
            _resourceFile = message.Files.First();

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
            if(_destinationOrganizationId != _localNodeIdentity.Id)
            {
                var sendResourceToPeerMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendResourceToPeerMessage>>();

                var message = new SendResourceToPeerMessage()
                {
                    TicketId = _ticketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
              
                };

                sendResourceToPeerMessageProducer.PublishMessage(message);
            }
            else
            {
                var postResourceToOperatorMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostResourceToOperatorMessage>>();

                var message = new PostResourceToOperatorMessage()
                {
                    TicketId = _ticketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    ResourceId = _resourceId,
                    Files = new List<FileDTO>() { _resourceFile },
                };

                postResourceToOperatorMessageProducer.PublishMessage(message);
            }

        }

        public override void OnPostResourceToOperatorResult(PostResourceToOperatorResultMessage message)
        {
            SendActionResult();
        }


        #endregion

        #region Resource sending to repository
        private void SendResourceToRepositoryMs()
        {
            if (_destinationOrganizationId != _localNodeIdentity.Id)
            {
                var sendResourceToPeerMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendResourceToPeerMessage>>();

                var message = new SendResourceToPeerMessage()
                {
                    TicketId = _ticketId,
                    TimeToLive = TimeSpan.FromMinutes(1),

                };

                sendResourceToPeerMessageProducer.PublishMessage(message);
            }
            else
            {
                var postResourceToRepoMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostResourceToRepoMessage>>();

                var message = new PostResourceToRepoMessage()
                {
                    TicketId = _ticketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    OrganizationId = _organizationId,
                    RepositoryId = (Guid)_destinationRepositoryId,
                    Name = "test",
                    ResourceType = "test",
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


        public override void OnSendResourceToPeerResult(SendResourceToPeerResultMessage message)
        {

        }


        private void SendActionResult()
        {
            var actionResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ActionResultMessage>>();

            var actionResultDto = new ActionResultDTO()
            {
                ActionResult = ActionResult.Completed,
                ExecutionId = _executionId,
                StepId = _stepId,
                Message = "Step completed"
            };


            var message = new ActionResultMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ActionResult = actionResultDto
            };

            actionResultProducer.PublishMessage(message);

            EndProcess();
        }
    }
}
