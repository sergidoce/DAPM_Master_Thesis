using DAPM.Orchestrator.Services;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes
{
    public class RegistryUpdateProcess : OrchestratorProcess
    {
        private RegistryUpdateDTO _registryUpdateDTO;
        private IdentityDTO _senderIdentityDTO;

        public RegistryUpdateProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, RegistryUpdateDTO registryUpdateDTO, IdentityDTO senderIdentity)
            : base(engine, serviceProvider, ticketId)
        {
            _registryUpdateDTO = registryUpdateDTO;
            _senderIdentityDTO = senderIdentity;
        }

        public override void StartProcess()
        {
            var applyRegistryUpdateProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ApplyRegistryUpdateMessage>>();

            var message = new ApplyRegistryUpdateMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                RegistryUpdate = _registryUpdateDTO
               
            };

            applyRegistryUpdateProducer.PublishMessage(message);
        }

        public override void OnApplyRegistryUpdateResult(ApplyRegistryUpdateResult message)
        {
            var sendRegistryUpdateAckProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendRegistryUpdateAckMessage>>();

            var identityDTO = new IdentityDTO()
            {
                Id = _localPeerIdentity.Id,
                Name = _localPeerIdentity.Name,
                Domain = _localPeerIdentity.Domain,
            };

            var ackMessage = new SendRegistryUpdateAckMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                SenderPeerIdentity = identityDTO,
                TargetPeerDomain = _senderIdentityDTO.Domain,
                RegistryUpdateAck = new RegistryUpdateAckDTO() { IsCompleted = true }
            };

            sendRegistryUpdateAckProducer.PublishMessage(ackMessage);

            EndProcess();
        }
    }
}
