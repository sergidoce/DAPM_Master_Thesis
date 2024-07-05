namespace DAPM.PeerApi.Models.ActionsDtos
{
    public class ActionResultDto
    {
        public Guid ProcessId { get; set; }
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public int ActionResult { get; set; }
        public string Message { get; set; }
    }
}
