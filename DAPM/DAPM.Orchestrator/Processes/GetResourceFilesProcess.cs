using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using System.Resources;

namespace DAPM.Orchestrator.Processes
{
    public class GetResourceFilesProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid _repositoryId;
        private Guid _resourceId;
        public GetResourceFilesProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid organizationId,
            Guid repositoryId, Guid resourceId)
            : base(engine, serviceProvider, ticketId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _resourceId = resourceId;
        }

        public override void StartProcess()
        {
            var getResourceFilesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetResourceFilesFromRepoMessage>>();

            var message = new GetResourceFilesFromRepoMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                RepositoryId = _repositoryId,
                ResourceId = _resourceId
            };

            getResourceFilesProducer.PublishMessage(message);
        }


        public override void OnGetResourceFilesFromRepoResult(GetResourceFilesFromRepoResultMessage message)
        {
            var getResourceFilesResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetResourceFilesProcessResult>>();

            var resultMessage = new GetResourceFilesProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Files = message.Files,
            };

            getResourceFilesResultProducer.PublishMessage(resultMessage);
        }


    }
}
