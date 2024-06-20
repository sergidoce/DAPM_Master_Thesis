using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.Other;

namespace DAPM.Orchestrator.Consumers
{
    public class ActionResultReceivedConsumer : IQueueConsumer<ActionResultReceivedMessage>
    {
        public Task ConsumeAsync(ActionResultReceivedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
