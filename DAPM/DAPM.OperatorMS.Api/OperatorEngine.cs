using DAPM.OperatorMS.Api.Services;
using DAPM.OperatorMS.Api.Services.Interfaces;
using RabbitMQLibrary.Messages.Operator;

namespace DAPM.OperatorMS.Api
{
    public class OperatorEngine : IOperatorEngine
    {
        private Dictionary<Guid, OperatorExecution> _executions;
        private IServiceProvider _serviceProvider;
        private IDockerService _dockerService;

        public OperatorEngine(IServiceProvider serviceProvider, IDockerService dockerService) 
        {
            _executions = new Dictionary<Guid, OperatorExecution>();
            _serviceProvider = serviceProvider;
            _dockerService = dockerService;
        }

        public void DeleteExecution(Guid executionId) 
        {
            _executions.Remove(executionId);
        }

        public OperatorExecution GetExecution(Guid executionId)
        {
            return _executions[executionId];
        }

        public async Task<bool> StartOperatorExecution(ExecuteOperatorMessage message)
        {
            var operatorExecution = new OperatorExecution(this, message.ProcessId, message.PipelineExecutionId, message.OutputResourceId, message.SourceCode, message.Dockerfile, _dockerService);
            _executions[message.ProcessId] = operatorExecution;
            bool succeeded = await operatorExecution.StartExecution();
            return succeeded;
        }
    }
}
