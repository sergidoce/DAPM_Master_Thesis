using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces
{
    public interface IPipelineOrchestrationEngine
    {
        public Guid CreateNewExecutionInstance(PipelineDTO pipelineDTO);
        public int StartPipelineExecution(Guid executionId);
    }
}
