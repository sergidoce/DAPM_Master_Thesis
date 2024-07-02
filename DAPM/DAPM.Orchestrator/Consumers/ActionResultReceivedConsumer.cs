using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.Other;

namespace DAPM.Orchestrator.Consumers
{
    public class ActionResultReceivedConsumer : IQueueConsumer<ActionResultReceivedMessage>
    {
        IOrchestratorEngine _engine;

        public ActionResultReceivedConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }


        public Task ConsumeAsync(ActionResultReceivedMessage message)
        {
            OrchestratorProcess process = _engine.GetProcess(message.StepId);
            process.OnActionResultFromPeer(message);
            return Task.CompletedTask;
        }
    }
}
