﻿
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class GetRepositoriesProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid? _repositoryId;
        public GetRepositoriesProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid organizationId, Guid? repositoryId) 
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
        }

        public override void StartProcess()
        {
            var getRepositoriesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetRepositoriesMessage>>();

            var message = new GetRepositoriesMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId
            };

            getRepositoriesProducer.PublishMessage(message);
        }

        public override void OnGetRepositoriesFromRegistryResult(GetRepositoriesResultMessage message)
        {

            var getRepositoriesProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetRepositoriesProcessResult>>();

            var processResultMessage = new GetRepositoriesProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Repositories = message.Repositories
            };

            getRepositoriesProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}
