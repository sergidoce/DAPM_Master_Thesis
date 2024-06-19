using DAPM.PipelineOrchestratorMS.Api.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces
{
    public interface IPipelineExecution
    {
        public void StartExecution();
        public void ProcessActionResult(ActionResultDTO actionResultDto);
    }
}
