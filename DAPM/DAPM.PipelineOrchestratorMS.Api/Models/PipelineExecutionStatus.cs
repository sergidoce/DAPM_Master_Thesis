using DAPM.PipelineOrchestratorMS.Api.Engine;

namespace DAPM.PipelineOrchestratorMS.Api.Models
{
    public class PipelineExecutionStatus
    {
        public TimeSpan ExecutionTime { get; set; }
        public List<StepStatus> CurrentSteps { get; set; }
        public PipelineExecutionState State { get; set; }
    }
}
