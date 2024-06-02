﻿
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class GetResourcesProcess : OrchestratorProcess
    {
        private int _organizationId;
        private int _repositoryId;
        private int? _resourceId;
        public GetResourcesProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, int organizationId,
            int repositoryId, int? resourceId) 
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _resourceId = resourceId;
        }

        public override void StartProcess()
        {
            var getResourceOfRepoProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetResourcesMessage>>();

            var message = new GetResourcesMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
            };

            getResourceOfRepoProducer.PublishMessage(message);
        }

        public override void OnGetResourcesFromRegistryResult(GetResourcesResultMessage message)
        {

            var getResourcesProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetResourcesProcessResult>>();

            var processResultMessage = new GetResourcesProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Resources = message.Resources
            };

            getResourcesProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}
