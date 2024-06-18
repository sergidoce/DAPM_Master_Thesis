using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Models
{
    public class ExecuteOperatorStep : Step
    {
        public EngineResource OperatorResource { get; set; }
        public List<EngineResource> InputResources { get; set; }

        public Guid TargetOrganization { get; set; }

        public ExecuteOperatorStep(Guid id, IServiceProvider serviceProvider) : base(id, serviceProvider)
        {
           InputResources = new List<EngineResource>();
        }


        public override void Execute()
        {
            var executeOperatorRequestProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ExecuteOperatorActionRequest>>();

            var data = new ExecuteOperatorActionDTO()
            {
                ExecutionId = ExecutionId,
                StepId = Id
            };


            var message = new ExecuteOperatorActionRequest()
            {
                TicketId = Id,
                TimeToLive = TimeSpan.FromMinutes(1),
                Data = data
            };

            executeOperatorRequestProducer.PublishMessage(message);
        }
    }
}
