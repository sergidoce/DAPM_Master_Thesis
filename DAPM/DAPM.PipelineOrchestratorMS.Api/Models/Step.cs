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
        public Guid ExecutionId { get; set; }
        public StepStatus Status { get; set; }
        public List<Guid> PrerequisiteSteps { get; set; }

        protected IServiceProvider _serviceProvider;
        protected IServiceScope _serviceScope;

        public Step(Guid executionId, IServiceProvider serviceProvider)
        {
            Id = Guid.NewGuid();
            Status = StepStatus.NotStarted;
            PrerequisiteSteps = new List<Guid>();
            _serviceProvider = serviceProvider;
            _serviceScope = _serviceProvider.CreateScope();
            ExecutionId = executionId;
        }

        public abstract void Execute();
     

    }
}
