using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.PeerApi.PipelineExecution;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes.PipelineActions
{
    public class ExecuteOperatorActionProcess : OrchestratorProcess
    {
        private Guid _stepId;
        private Guid _executionId;

        private Guid _operatorOrganizationId;
        private Guid? _operatorRepositoryId;
        private Guid _operatorResourceId;

        private List<ResourceDTO> _inputResources;
        private Guid _outputResourceId;

        private ResourceDTO _operatorResource;
        private FileDTO _operatorSourceCode;
        private FileDTO _operatorDockerFile;

        private IdentityDTO _orchestratorIdentity;

        private Guid? _senderProcessId;

        public ExecuteOperatorActionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, 
            Guid processId, Guid? senderProcessId, ExecuteOperatorActionDTO data, IdentityDTO orchestratorIdentity) 
            : base(engine, serviceProvider, processId)
        {
            _stepId = data.StepId;
            _executionId = data.ExecutionId;

            _operatorOrganizationId = data.OperatorResource.OrganizationId;
            _operatorRepositoryId = data.OperatorResource.RepositoryId;
            _operatorResourceId = data.OperatorResource.Id;

            _inputResources = data.InputResources;
            _outputResourceId = data.OutputResourceId;

            _orchestratorIdentity = orchestratorIdentity;

            _senderProcessId = senderProcessId;
        }


        public override void StartProcess()
        {
            var getResourceFilesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOperatorFilesFromRepoMessage>>();

            var message = new GetOperatorFilesFromRepoMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _operatorOrganizationId,
                RepositoryId = (Guid)_operatorRepositoryId,
                ResourceId = _operatorResourceId,
            };

            getResourceFilesProducer.PublishMessage(message);
        }

        public override void OnGetOperatorFilesFromRepoResult(GetOperatorFilesFromRepoResultMessage message)
        {
            _operatorResource = message.SourceCodeResource;
            _operatorSourceCode = message.SourceCodeResource.File;
            _operatorDockerFile = message.DockerfileFile;

            var executeOperatorProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ExecuteOperatorMessage>>();

            var executeOperatorMessage = new ExecuteOperatorMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                PipelineExecutionId = _executionId,
                OutputResourceId = _outputResourceId,
                Dockerfile = _operatorDockerFile,
                SourceCode = _operatorResource
            };

            executeOperatorProducer.PublishMessage(executeOperatorMessage);
        }

        public override void OnExecuteOperatorResult(ExecuteOperatorResultMessage message)
        {
            var actionResultDto = new ActionResultDTO()
            {
                ActionResult = ActionResult.Completed,
                ExecutionId = _executionId,
                StepId = _stepId,
                Message = "Step completed"
            };

            if (_localPeerIdentity.Id != _orchestratorIdentity.Id)
            {
                var sendActionResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendActionResultMessage>>();

                var actionResultMessage = new SendActionResultMessage()
                {
                    SenderProcessId = (Guid)_senderProcessId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    TargetPeerDomain = _orchestratorIdentity.Domain,
                    ActionResult = actionResultDto
                };

                sendActionResultProducer.PublishMessage(actionResultMessage);
            }
            else
            {
                var actionResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ActionResultMessage>>();

                var actionResultMessage = new ActionResultMessage()
                {
                    TimeToLive = TimeSpan.FromMinutes(1),
                    ActionResult = actionResultDto
                };

                actionResultProducer.PublishMessage(actionResultMessage);
            }


            EndProcess();
        }

      
    }
}
