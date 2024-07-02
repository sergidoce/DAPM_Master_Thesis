using DAPM.Orchestrator.Services;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class TransferDataActionRequestConsumer : IQueueConsumer<TransferDataActionRequest>
    {
        private IOrchestratorEngine _engine;
        private IServiceScope _serviceScope;

        public TransferDataActionRequestConsumer(IOrchestratorEngine engine,
            IServiceProvider serviceProvider)
        {
            _engine = engine;
            _serviceScope = serviceProvider.CreateScope();
        }

        public Task ConsumeAsync(TransferDataActionRequest message)
        {
            var identityService = _serviceScope.ServiceProvider.GetRequiredService<IIdentityService>();
            var identity = identityService.GetIdentity();
            var destinationOrganizationId = message.Data.DestinationOrganizationId;

            if(identity.Id != destinationOrganizationId)
            {
                _engine.StartSendTransferDataActionProcess(message.TicketId, message.Data);
            }
            else
            {
                _engine.StartTransferDataActionProcess(message.TicketId, message.OrchestratorIdentity, message.Data);
            }

            return Task.CompletedTask;
        }
    }
}
