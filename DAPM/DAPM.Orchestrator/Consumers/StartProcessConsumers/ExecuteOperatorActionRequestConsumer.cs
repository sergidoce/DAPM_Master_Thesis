using DAPM.Orchestrator.Services;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class ExecuteOperatorActionRequestConsumer : IQueueConsumer<ExecuteOperatorActionRequest>
    {
        private IOrchestratorEngine _engine;
        private IServiceScope _serviceScope;

        public ExecuteOperatorActionRequestConsumer(IOrchestratorEngine engine,
            IServiceProvider serviceProvider)
        {
            _engine = engine;
            _serviceScope = serviceProvider.CreateScope();
        }

        public Task ConsumeAsync(ExecuteOperatorActionRequest message)
        {
            var identityService = _serviceScope.ServiceProvider.GetRequiredService<IIdentityService>();
            var identity = identityService.GetIdentity();
            var destinationOrganizationId = message.Data.OperatorResource.OrganizationId;

            var orchestratorIdentity = new IdentityDTO();

            if (message.OrchestratorIdentity == null)
            {
                orchestratorIdentity.Id = identity.Id;
                orchestratorIdentity.Name = identity.Name;
                orchestratorIdentity.Domain = identity.Domain;
            }
            else
            {
                orchestratorIdentity = message.OrchestratorIdentity;
            }

            if (identity.Id != destinationOrganizationId)
            {
                _engine.StartSendExecuteOperatorActionProcess(message.Data);
            }
            else
            {
                _engine.StartExecuteOperatorActionProcess(message.SenderProcessId, orchestratorIdentity, message.Data);
            }
            
            return Task.CompletedTask;
        }
    }
}
