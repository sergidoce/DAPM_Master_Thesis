using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class CreateRepositoryRequestConsumer : IQueueConsumer<CreateRepositoryRequest>
    {
        public Task ConsumeAsync(CreateRepositoryRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
