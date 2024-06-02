using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
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

        public Guid CreateNewExecutionInstance(Pipeline pipeline)
        {
            Guid guid = Guid.NewGuid();

            var pipelineExecution = new PipelineExecution(pipeline, _serviceProvider);
            
            _pipelineExecutions[guid] = pipelineExecution;
            _logger.LogInformation($"A new execution instance has been created");
            

            return guid;
        }

        public int StartPipelineExecution(Guid executionId)
        {
            throw new NotImplementedException();
        }
    }
}
