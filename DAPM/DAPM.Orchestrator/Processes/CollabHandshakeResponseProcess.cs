using DAPM.Orchestrator.Services.Models;
using DAPM.Orchestrator.Services;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Messages.PeerApi.Handshake;

namespace DAPM.Orchestrator.Processes
{
    public class CollabHandshakeResponseProcess : OrchestratorProcess
    {
        private ILogger<CollabHandshakeResponseProcess> _logger;
        private IIdentityService _identityService;
        private Identity _requesterPeerIdentity;
        private Identity _localPeerIdentity;

        public CollabHandshakeResponseProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
             Guid processId, Identity requesterPeerIdentity)
            : base(engine, serviceProvider, processId)
        {
            _requesterPeerIdentity = requesterPeerIdentity;
            _identityService = serviceProvider.GetRequiredService<IIdentityService>();
            _localPeerIdentity = _identityService.GetIdentity();
            _logger = serviceProvider.GetRequiredService<ILogger<CollabHandshakeResponseProcess>>();
        }

        public override void StartProcess()
        {
            _logger.LogInformation("HANDSHAKE RESPONSE PROCESS STARTED");
            var sendRequestResponseProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendHandshakeRequestResponseMessage>>();

            var isRequestAccepted = true;

            var identityDto = new IdentityDTO()
            {
                Id = _localPeerIdentity.Id,
                Name = _localPeerIdentity.Name,
                Domain = _localPeerIdentity.Domain,
            };

            var message = new SendHandshakeRequestResponseMessage()
            {
                SenderProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                TargetDomain = _requesterPeerIdentity.Domain,
                SenderPeerIdentity = identityDto,
                IsRequestAccepted = isRequestAccepted,
            };

            sendRequestResponseProducer.PublishMessage(message);

            if(isRequestAccepted == false)
            {
                EndProcess();
            }
        }

 
        public override void OnRegistryUpdate(RegistryUpdateMessage message)
        {
            _logger.LogInformation("REGISTRY UPDATE RECEIVED");

            var applyRegistryUpdateProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ApplyRegistryUpdateMessage>>();


            var applyRegistryUpdateMessage = new ApplyRegistryUpdateMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                RegistryUpdate = message.RegistryUpdate

            };

            applyRegistryUpdateProducer.PublishMessage(applyRegistryUpdateMessage);
        }

        public override void OnApplyRegistryUpdateResult(ApplyRegistryUpdateResult message)
        {
         
            var getEntriesFromOrgProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetEntriesFromOrgMessage>>();

            var getEntriesFromOrgMessage = new GetEntriesFromOrgMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = (Guid)_localPeerIdentity.Id
            };

            getEntriesFromOrgProducer.PublishMessage(getEntriesFromOrgMessage);
        }

        public override void OnGetEntriesFromOrgResult(GetEntriesFromOrgResult message)
        {
            var sendRegistryUpdateProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendRegistryUpdateMessage>>();
            var registryUpdate = new RegistryUpdateDTO()
            {
                Organizations = new List<OrganizationDTO>() { message.Organization },
                Repositories = message.Repositories,
                Resources = message.Resources,
                Pipelines = message.Pipelines,
            };

            var senderIdentityDto = new IdentityDTO()
            {
                Id = _localPeerIdentity.Id,
                Name = _localPeerIdentity.Name,
                Domain = _localPeerIdentity.Domain,
            };

            var sendRegistryUpdateMessage = new SendRegistryUpdateMessage()
            {
                SenderProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                SenderPeerIdentity = senderIdentityDto,
                TargetPeerDomain = _requesterPeerIdentity.Domain,
                RegistryUpdate = registryUpdate,
                IsPartOfHandshake = true

            };

            sendRegistryUpdateProducer.PublishMessage(sendRegistryUpdateMessage);
        }

        public override void OnRegistryUpdateAck(RegistryUpdateAckMessage message)
        {

            _logger.LogInformation("HANDSHAKE ACK RECEIVED");
            var sendHandshakeAckProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendRegistryUpdateAckMessage>>();

            var senderIdentityDto = new IdentityDTO()
            {
                Id = _localPeerIdentity.Id,
                Name = _localPeerIdentity.Name,
                Domain = _localPeerIdentity.Domain,
            };

            var sendHandshakeAckMessage = new SendRegistryUpdateAckMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                SenderPeerIdentity = senderIdentityDto,
                TargetPeerDomain = _requesterPeerIdentity.Domain,
                RegistryUpdateAck = new RegistryUpdateAckDTO() { IsCompleted = true }

            };

            sendHandshakeAckProducer.PublishMessage(sendHandshakeAckMessage);

            EndProcess();
        }
    }
}
