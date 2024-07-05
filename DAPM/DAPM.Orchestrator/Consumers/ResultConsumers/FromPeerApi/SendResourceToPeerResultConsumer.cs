using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromPeerApi
{
    public class SendResourceToPeerResultConsumer : IQueueConsumer<SendResourceToPeerResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public SendResourceToPeerResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(SendResourceToPeerResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.SenderProcessId);
            process.OnSendResourceToPeerResult(message);

            return Task.CompletedTask;
        }
    }
}
