using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Models
{
    public class ExecuteOperatorStep : Step
    {
        public EngineResource OperatorResource { get; set; }
        public List<EngineResource> InputResources { get; set; }
        public Guid OutputResourceId { get; set; }

        public ExecuteOperatorStep(Guid id, IServiceProvider serviceProvider) : base(id, serviceProvider)
        {
           InputResources = new List<EngineResource>();
           OutputResourceId = Guid.NewGuid();    
        }


        public override void Execute()
        {
            var executeOperatorRequestProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<ExecuteOperatorActionRequest>>();


            var operatorResourceDto = new ResourceDTO()
            {
                Id = OperatorResource.ResourceId,
                RepositoryId = (Guid)OperatorResource.RepositoryId,
                OrganizationId = OperatorResource.OrganizationId,
            };

            var inputResourceDtos = new List<ResourceDTO>();

            foreach(var resource in InputResources)
            {
                inputResourceDtos.Add(new ResourceDTO()
                {
                    Id = resource.ResourceId,
                    RepositoryId = resource.RepositoryId,
                    OrganizationId = resource.OrganizationId,
                });
            }


            var data = new ExecuteOperatorActionDTO()
            {
                ExecutionId = ExecutionId,
                StepId = Id,
                OperatorResource = operatorResourceDto,
                InputResources = inputResourceDtos,
                OutputResourceId = OutputResourceId,

            };


            var message = new ExecuteOperatorActionRequest()
            {
                SenderProcessId = null,
                TimeToLive = TimeSpan.FromMinutes(1),
                Data = data
            };

            executeOperatorRequestProducer.PublishMessage(message);
        }
    }
}
