using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;
using System.Resources;

namespace DAPM.Orchestrator.Processes
{
    public class GetResourceFilesProcess : OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid _repositoryId;
        private Guid _resourceId;

        private Guid _ticketId;
        public GetResourceFilesProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid processId,
            Guid organizationId,
            Guid repositoryId, Guid resourceId)
            : base(engine, serviceProvider, processId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _resourceId = resourceId;

            _ticketId = ticketId;
        }

        public override void StartProcess()
        {
            var getResourceFilesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetResourceFilesFromRepoMessage>>();

            var message = new GetResourceFilesFromRepoMessage()
            {
                ProcessId = _processId,
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
                Files = new List<FileDTO>() { message.Resource.File},
            };

            getResourceFilesResultProducer.PublishMessage(resultMessage);
        }


    }
}
