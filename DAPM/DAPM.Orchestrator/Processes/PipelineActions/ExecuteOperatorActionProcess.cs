using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
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

        public ExecuteOperatorActionProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, ExecuteOperatorActionDTO data) 
            : base(engine, serviceProvider, ticketId)
        {
            _stepId = data.StepId;
            _executionId = data.ExecutionId;

            _operatorOrganizationId = data.OperatorResource.OrganizationId;
            _operatorRepositoryId = data.OperatorResource.RepositoryId;
            _operatorResourceId = data.OperatorResource.Id;

            _inputResources = data.InputResources;
            _outputResourceId = data.OutputResourceId;
        }


        public override void StartProcess()
        {
            var getResourceFilesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOperatorFilesFromRepoMessage>>();

            var message = new GetOperatorFilesFromRepoMessage()
            {
                TicketId = _ticketId,
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
            List<Guid> inputResourceIds = new List<Guid>();
            foreach (ResourceDTO inputResource in _inputResources) 
            {
                inputResourceIds.Add(inputResource.Id);
            }

            var executeOperatorMessage = new ExecuteOperatorMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                PipelineExecutionId = _executionId,
                OutputResourceId = _outputResourceId,
                Dockerfile = _operatorDockerFile,
                SourceCode = _operatorResource,
                InputResourceIds = inputResourceIds,
            };

            executeOperatorProducer.PublishMessage(executeOperatorMessage);
        }

        public override void OnExecuteOperatorResult(ExecuteOperatorResultMessage message)
        {
            var actionResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ActionResultMessage>>();


            var actionResultDto = new ActionResultDTO()
            {
                ActionResult = ActionResult.Completed,
                ExecutionId = _executionId,
                StepId = _stepId,
                Message = "Step completed"
            };


            var resultMessage = new ActionResultMessage()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ActionResult = actionResultDto
            };

            actionResultProducer.PublishMessage(resultMessage);

            EndProcess();
        }

      
    }
}
