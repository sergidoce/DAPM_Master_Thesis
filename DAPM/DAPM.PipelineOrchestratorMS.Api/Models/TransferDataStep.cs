using Microsoft.Extensions.DependencyInjection;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Models
{

    public enum StorageMode
    {
        Permanent,
        Temporary
    }


    public class TransferDataStep : Step
    {

        private EngineResource _resourceToTransfer;
        private Guid _destinationOrganization;
        private Guid? _destinationRepository;

        private StorageMode _sourceStorageMode;
        private StorageMode _destinationStorageMode;

        private string? _destinationName;

        public TransferDataStep(EngineResource resourceToTransfer,
            Guid destinationOrganization,
            Guid? destinationRepository,
            StorageMode sourceStorageMode,
            StorageMode destinationStorageMode,
            string? destinationName,
            Guid executionId,
            IServiceProvider serviceProvider) : base(executionId, serviceProvider)
        {
            _resourceToTransfer = resourceToTransfer;
            _destinationOrganization = destinationOrganization;
            _destinationRepository = destinationRepository;
            _sourceStorageMode = sourceStorageMode;
            _destinationStorageMode = destinationStorageMode;
            _destinationName = destinationName;
        }

        public EngineResource GetResourceToTransfer()
        {
            return _resourceToTransfer;
        }

        public override void Execute()
        {
            var transferDataRequestProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<TransferDataActionRequest>>();

            var data = new TransferDataActionDTO()
            {
                ExecutionId = ExecutionId,
                StepId = Id,
                OriginOrganizationId = _resourceToTransfer.OrganizationId,
                OriginRepositoryId = _resourceToTransfer.RepositoryId,
                OriginResourceId = _resourceToTransfer.ResourceId,

                SourceStorageMode = (int)_sourceStorageMode,
                DestinationStorageMode = (int)_destinationStorageMode,

                DestinationName = _destinationName,

                DestinationOrganizationId = _destinationOrganization,
                DestinationRepositoryId = _destinationRepository,

            };

            var message = new TransferDataActionRequest()
            {
                TicketId = Id,
                TimeToLive = TimeSpan.FromMinutes(1),
                Data = data
            };

            transferDataRequestProducer.PublishMessage(message);
        }
    }
}
