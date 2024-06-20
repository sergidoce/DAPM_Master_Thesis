using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes
{
    public class CreatePipelineExecutionProcess : OrchestratorProcess
    {
        // Pipeline to execute ids
        private Guid _organizationId;
        private Guid _repositoryId;
        private Guid _pipelineId;

        //Pipeline to execute
        private PipelineDTO? _pipelineDTO;

        public CreatePipelineExecutionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid organizationId, Guid repositoryId, Guid pipelineId) 
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _pipelineId = pipelineId;
        }

        public override void StartProcess()
        {
            var getPipelinesFromRepoMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelinesFromRepoMessage>>();

            var message = new GetPipelinesFromRepoMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                RepositoryId = _repositoryId,
                PipelineId = _pipelineId,
            
            };

            getPipelinesFromRepoMessageProducer.PublishMessage(message);
        }

        public override void OnGetPipelinesFromRepoResult(GetPipelinesFromRepoResultMessage message)
        {
            _pipelineDTO = message.Pipelines.FirstOrDefault();

            var createInstanceExecutionMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<CreateInstanceExecutionMessage>>();

            var createInstanceExecutionMessage = new CreateInstanceExecutionMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Pipeline = _pipelineDTO
            };

            createInstanceExecutionMessageProducer.PublishMessage(createInstanceExecutionMessage);

        }

        public override void OnCreatePipelineExecutionResult(CreatePipelineExecutionResultMessage message)
        {
            var postItemProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostItemProcessResult>>();

            var itemsIds = new ItemIds()
            {
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
                PipelineId = _pipelineId,
                ExecutionId = message.PipelineExecutionId,
            };

            var postItemProcessResultMessage = new PostItemProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ItemIds = itemsIds,
                ItemType = "Pipeline Execution",
                Message = "The item was posted successfully",
                Succeeded = true
            };

            postItemProcessResultProducer.PublishMessage(postItemProcessResultMessage);

            EndProcess();
        }
    }
}
