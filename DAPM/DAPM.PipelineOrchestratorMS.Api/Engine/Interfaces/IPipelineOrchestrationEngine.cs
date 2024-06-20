using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces
{
    public interface IPipelineOrchestrationEngine
    {
        public Guid CreatePipelineExecutionInstance(Pipeline pipelineDTO);
        public void ExecutePipelineStartCommand(Guid executionId);
        public void ProcessActionResult(ActionResultDTO actionResultDto);
    }
}
