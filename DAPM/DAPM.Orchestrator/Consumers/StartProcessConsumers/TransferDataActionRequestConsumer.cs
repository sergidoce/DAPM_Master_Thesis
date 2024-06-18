using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class TransferDataActionRequestConsumer : IQueueConsumer<TransferDataActionRequest>
    {
        IOrchestratorEngine _engine;
        public TransferDataActionRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(TransferDataActionRequest message)
        {
            _engine.StartTransferDataActionProcess(message.TicketId, message.Data);
            return Task.CompletedTask;
        }
    }
}
