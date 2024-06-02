using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class GetPipelinesRequestConsumer : IQueueConsumer<GetPipelinesRequest>
    {
        IOrchestratorEngine _engine;
        public GetPipelinesRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }
        public Task ConsumeAsync(GetPipelinesRequest message)
        {
            _engine.StartGetPipelinesProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.PipelineId);
            return Task.CompletedTask;
        }
    }
}
