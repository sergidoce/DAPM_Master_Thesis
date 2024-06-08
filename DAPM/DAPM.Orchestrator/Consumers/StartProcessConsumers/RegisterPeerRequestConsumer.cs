using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class RegisterPeerRequestConsumer : IQueueConsumer<RegisterPeerRequest>
    {
        IOrchestratorEngine _engine;

        public RegisterPeerRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(RegisterPeerRequest message)
        {
            _engine.StartRegisterPeerProcess(message.TicketId, message.IntroductionPeerAddress, message.LocalPeerAddress, message.PeerName);
            return Task.CompletedTask;
        }
    }
}
