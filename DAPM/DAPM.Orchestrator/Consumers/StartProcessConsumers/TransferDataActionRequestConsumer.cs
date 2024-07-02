using DAPM.Orchestrator.Services;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Models;

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
            var originOrganizationId = message.Data.OriginOrganizationId;

            var orchestratorIdentity = new IdentityDTO();

            if(message.OrchestratorIdentity == null)
            {
                orchestratorIdentity.Id = identity.Id;
                orchestratorIdentity.Name = identity.Name;
                orchestratorIdentity.Domain = identity.Domain;
            }
            else
            {
                orchestratorIdentity = message.OrchestratorIdentity;
            }

            if(identity.Id != originOrganizationId)
            {
                _engine.StartSendTransferDataActionProcess(message.TicketId, message.Data);
            }
            else
            {
                _engine.StartTransferDataActionProcess(message.TicketId, orchestratorIdentity, message.Data);
            }

            return Task.CompletedTask;
        }
    }
}
