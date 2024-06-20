using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class PostOperatorRequestConsumer : IQueueConsumer<PostOperatorRequest>
    {
        IOrchestratorEngine _engine;
        public PostOperatorRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }
        public Task ConsumeAsync(PostOperatorRequest message)
        {
            _engine.StartPostOperatorProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.Name, message.ResourceType, message.SourceCodeFile,
                message.DockerfileFile);
            return Task.CompletedTask;
        }
    }
}
