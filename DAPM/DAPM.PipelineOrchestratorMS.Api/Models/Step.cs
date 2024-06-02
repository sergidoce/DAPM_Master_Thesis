namespace DAPM.PipelineOrchestratorMS.Api.Models
{

    public enum StepStatus
    {
        NotStarted,
        Running,
        Completed,
        Error
    }


    public abstract class Step
    {
        public Guid Id { get; set; }
        public StepStatus Status { get; set; }
        public List<Guid> PrerequisiteSteps { get; set; }

        private IServiceProvider _serviceProvider;

        public Step(IServiceProvider serviceProvider)
        {
            Id = Guid.NewGuid();
            Status = StepStatus.NotStarted;
            PrerequisiteSteps = new List<Guid>();
            _serviceProvider = serviceProvider;
        }

        public abstract void Execute();
     

    }
}
