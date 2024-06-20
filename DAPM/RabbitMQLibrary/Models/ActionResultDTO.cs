namespace DAPM.PipelineOrchestratorMS.Api.Models
{

    public enum ActionResult
    {
        Completed,
        Faulted,
    }

    public class ActionResultDTO
    {
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public ActionResult ActionResult { get; set; }
        public string Message { get; set; }

    }
}
