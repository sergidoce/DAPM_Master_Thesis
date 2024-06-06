using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.Repository;

namespace DAPM.Orchestrator.Processes
{
    public class GetPipelinesProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid _repositoryId;
        private Guid? _pipelineId;

        public GetPipelinesProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId,
            Guid organizationId, Guid repositoryId, Guid? pipelineId)
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _pipelineId = pipelineId;
        }

        public override void StartProcess()
        {
            var getPipelinesFromRepoProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelinesFromRepoMessage>>();

            var message = new GetPipelinesFromRepoMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                RepositoryId = _repositoryId,
                PipelineId = _pipelineId,
            };

            getPipelinesFromRepoProducer.PublishMessage(message);
        }

        public override void OnGetPipelinesFromRepoResult(GetPipelinesFromRepoResultMessage message)
        {

            var getPipelinesProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelinesProcessResult>>();

            foreach(var pipeline in message.Pipelines)
            {
                pipeline.OrganizationId = _organizationId;
            }

            var resultMessage = new GetPipelinesProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Pipelines = message.Pipelines,
            };

            getPipelinesProcessResultProducer.PublishMessage(resultMessage);

        }
    }
}
