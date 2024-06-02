using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces
{
    public interface IPipelineOrchestrationEngine
    {
        public Guid CreateNewExecutionInstance(Pipeline pipelineDTO);
        public int StartPipelineExecution(Guid executionId);
    }
}
