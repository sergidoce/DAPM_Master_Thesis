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
    public class PostPipelineProcess : OrchestratorProcess
    {

        private Guid _organizationId;
        private Guid _repositoryId;
        private string _pipelineName;
        private Pipeline _pipeline;

        private PipelineDTO? _createdPipeline;

        private Dictionary<Guid, bool> _isRegistryUpdateCompleted;
        private int _registryUpdatesNotCompletedCounter;

        private Guid _ticketId;

        public PostPipelineProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid processId,
            Guid organizationId, Guid repositoryId, Pipeline pipeline, string name)
            : base(engine, serviceProvider, processId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _pipeline = pipeline;
            _pipelineName = name;

            _registryUpdatesNotCompletedCounter = 0;
            _isRegistryUpdateCompleted = new Dictionary<Guid, bool>();

            _ticketId = ticketId;
        }   
        public override void StartProcess()
        {
            var postPipelineToRepoProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostPipelineToRepoMessage>>();

            var message = new PostPipelineToRepoMessage()
            {
                ProcessId = _processId,
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
            var postPipelineToRegistryProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostPipelineToRegistryMessage>>();

            message.Pipeline.OrganizationId = _organizationId;

            var postPipelineToRegistryMessage = new PostPipelineToRegistryMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Pipeline = message.Pipeline,
            };

            postPipelineToRegistryProducer.PublishMessage(postPipelineToRegistryMessage);

        }

        public override void OnPostPipelineToRegistryResult(PostPipelineToRegistryResultMessage message)
        {
            _createdPipeline = message.Pipeline;

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
            var pipelinesList = new List<PipelineDTO>() { _createdPipeline };


            SendRegistryUpdates(targetOrganizations,
                Enumerable.Empty<OrganizationDTO>(),
                Enumerable.Empty<RepositoryDTO>(),
                Enumerable.Empty<ResourceDTO>(),
                pipelinesList);


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
                OrganizationId = _createdPipeline.OrganizationId,
                RepositoryId = _createdPipeline.RepositoryId,
                PipelineId = _createdPipeline.Id,
            };

            var postItemProcessResultMessage = new PostItemProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ItemIds = itemsIds,
                ItemType = "Pipeline",
                Message = "The item was posted successfully",
                Succeeded = true
            };

            postItemProcessResultProducer.PublishMessage(postItemProcessResultMessage);

            EndProcess();
        }

    }
}
