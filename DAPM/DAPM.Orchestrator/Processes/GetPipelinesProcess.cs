using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class GetPipelinesProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid _repositoryId;
        private Guid? _pipelineId;

        private Guid _ticketId;
        public GetPipelinesProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid processId,
            Guid organizationId, Guid repositoryId, Guid? pipelineId)
            : base(engine, serviceProvider, processId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _pipelineId = pipelineId;

            _ticketId = ticketId;
        }

        public override void StartProcess()
        {

            if(_pipelineId != null)
            {
                var getPipelinesFromRepoProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelinesFromRepoMessage>>();

                var message = new GetPipelinesFromRepoMessage()
                {
                    ProcessId = _processId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    RepositoryId = _repositoryId,
                    PipelineId = _pipelineId,
                };

                getPipelinesFromRepoProducer.PublishMessage(message);
            }

            else
            {
                var getPipelinesFromRegistryProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelinesMessage>>();

                var message = new GetPipelinesMessage()
                {
                    ProcessId = _processId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    OrganizationId = _organizationId,
                    RepositoryId = _repositoryId,
                };

                getPipelinesFromRegistryProducer.PublishMessage(message);
            }
            
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

        public override void OnGetPipelinesFromRegistryResult(GetPipelinesResultMessage message)
        {

            var getPipelinesProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelinesProcessResult>>();


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
