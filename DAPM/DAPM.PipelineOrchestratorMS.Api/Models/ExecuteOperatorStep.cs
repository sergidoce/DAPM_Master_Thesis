namespace DAPM.PipelineOrchestratorMS.Api.Models
{
    public class ExecuteOperatorStep : Step
    {
        public EngineResource OperatorResource { get; set; }
        public List<EngineResource> InputResources { get; set; }

        public Guid TargetOrganization { get; set; }

        public ExecuteOperatorStep(IServiceProvider serviceProvider) : base(serviceProvider)
        {
           InputResources = new List<EngineResource>();
        }


        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
