using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Engine
{
    public class PipelineOrchestrationEngine : IPipelineOrchestrationEngine
    {

        private readonly ILogger<IPipelineOrchestrationEngine> _logger;
        private IServiceProvider _serviceProvider;
        private Dictionary<Guid, IPipelineExecution> _pipelineExecutions;

        public PipelineOrchestrationEngine(ILogger<IPipelineOrchestrationEngine> logger, IServiceProvider serviceProvider)
        {
            _pipelineExecutions = new Dictionary<Guid, IPipelineExecution>();
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Guid CreatePipelineExecutionInstance(Pipeline pipeline)
        {
            Guid guid = Guid.NewGuid();

            var pipelineExecution = new PipelineExecution(guid, pipeline, _serviceProvider);
            
            _pipelineExecutions[guid] = pipelineExecution;
            _logger.LogInformation($"A new execution instance has been created");
            
            return guid;
        }

        public void ProcessActionResult(ActionResultDTO actionResultDto)
        {
            var execution = GetPipelineExecution(actionResultDto.ExecutionId);
            execution.ProcessActionResult(actionResultDto);
        }

        public void ExecutePipelineStartCommand(Guid executionId)
        {
            var execution = GetPipelineExecution(executionId);
            execution.StartExecution();
        }


        private IPipelineExecution GetPipelineExecution(Guid executionId)
        {
            return _pipelineExecutions[executionId];
        }

        public PipelineExecutionStatus GetPipelineExecutionStatus(Guid executionId)
        {
            return _pipelineExecutions[executionId].GetStatus();
        }
    }
}
