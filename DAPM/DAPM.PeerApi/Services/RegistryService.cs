using DAPM.PeerApi.Models;
using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Services
{
    public class RegistryService : IRegistryService
    {
        private IQueueProducer<RegistryUpdateMessage> _registryUpdateProducer;
        private IQueueProducer<RegistryUpdateAckMessage> _registryUpdateAckProducer;

        public RegistryService(IQueueProducer<RegistryUpdateMessage> registryUpdateProducer,
            IQueueProducer<RegistryUpdateAckMessage> handshakeAckProducer)
        {
            _registryUpdateProducer = registryUpdateProducer;
            _registryUpdateAckProducer = handshakeAckProducer;
        }

        public void OnRegistryUpdate(RegistryUpdateDto registryUpdateDto)
        {
            var registryUpdateDTO = new RegistryUpdateDTO()
            {
                Organizations = registryUpdateDto.Organizations,
                Repositories = registryUpdateDto.Repositories,
                Resources = registryUpdateDto.Resources,
                Pipelines = registryUpdateDto.Pipelines,
            };

            var message = new RegistryUpdateMessage()
            {
                SenderProcessId = (Guid)registryUpdateDto.SenderProcessId,
                SenderIdentity = registryUpdateDto.SenderIdentity,
                TimeToLive = TimeSpan.FromMinutes(1),
                RegistryUpdate = registryUpdateDTO,
                IsPartOfHandshake = registryUpdateDto.IsPartOfHandshake,
            };

            _registryUpdateProducer.PublishMessage(message);

        }

        public void OnRegistryUpdateAck(RegistryUpdateAckDto registryUpdateAck)
        {
            var rabbitMQDto = new RegistryUpdateAckDTO()
            {
                IsCompleted = registryUpdateAck.IsDone
            };

            var message = new RegistryUpdateAckMessage()
            {
                ProcessId = registryUpdateAck.ProcessId,
                TimeToLive = TimeSpan.FromMinutes(1),
                PeerSenderIdentity = registryUpdateAck.SenderIdentity,
                RegistryUpdateAck = rabbitMQDto
            };

            _registryUpdateAckProducer.PublishMessage(message);
        }
    }
}
