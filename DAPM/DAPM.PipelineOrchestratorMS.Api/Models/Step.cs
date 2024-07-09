using System.Diagnostics;

namespace DAPM.PipelineOrchestratorMS.Api.Models
{

    public enum StepState
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
        public StepState Status { get; set; }
        public List<Guid> PrerequisiteSteps { get; set; }

        protected IServiceProvider _serviceProvider;
        protected IServiceScope _serviceScope;

        protected Stopwatch _executionTimer { get; set; }

        public Step(Guid executionId, IServiceProvider serviceProvider)
        {
            Id = Guid.NewGuid();
            Status = StepState.NotStarted;
            PrerequisiteSteps = new List<Guid>();
            _serviceProvider = serviceProvider;
            _serviceScope = _serviceProvider.CreateScope();
            ExecutionId = executionId;
        }

        public abstract StepStatus GetStatus();

       

        public abstract void Execute();
     

    }
}
