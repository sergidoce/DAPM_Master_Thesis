namespace DAPM.PipelineOrchestratorMS.Api.Models
{

    public enum ActionResult
    {
        Completed,
        Faulted,
    }

    public class ActionAck
    {
        public Guid StepId { get; set; }
        public ActionResult ActionResult { get; set; }
        public string Message { get; set; }

    }
}
