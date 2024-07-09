namespace DAPM.PipelineOrchestratorMS.Api.Models
{
    public class StepStatus
    {
        public Guid StepId { get; set; }
        public Guid ExecutionerPeer { get; set; }
        public string StepType { get; set; }
        public TimeSpan ExecutionTime { get; set; }
    }
}
