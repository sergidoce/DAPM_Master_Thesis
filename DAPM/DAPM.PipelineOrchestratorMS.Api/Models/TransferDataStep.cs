namespace DAPM.PipelineOrchestratorMS.Api.Models
{

    public enum StorageMode
    {
        Permanent,
        Temporal
    }


    public class TransferDataStep : Step
    {

        private EngineResource _resourceToTransfer;
        private int _destinationOrganization;
        private int? _destinationRepository;

        private StorageMode _sourceStorageMode;
        private StorageMode _destinationStorageMode;

        public TransferDataStep(EngineResource resourceToTransfer,
            int destinationOrganization,
            int? destinationRepository,
            StorageMode sourceStorageMode,
            StorageMode destinationStorageMode,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _resourceToTransfer = resourceToTransfer;
            _destinationOrganization = destinationOrganization;
            _destinationRepository = destinationRepository;
            _sourceStorageMode = sourceStorageMode;
            _destinationStorageMode = destinationStorageMode;

        }

        public EngineResource GetResourceToTransfer()
        {
            return _resourceToTransfer;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
