using RabbitMQLibrary.Messages.Operator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using DAPM.OperatorMS.Api.Services.Interfaces;

namespace DAPM.OperatorMS.Api.Consumers
{
    public class GetExecutionOutputMessageConsumer : IQueueConsumer<GetExecutionOutputMessage>
    {
        private ILogger<GetExecutionOutputMessageConsumer> _logger;
        private IDockerService _dockerService;
        private IServiceProvider _serviceProvider;
        protected IServiceScope _serviceScope;

        public GetExecutionOutputMessageConsumer(ILogger<GetExecutionOutputMessageConsumer> logger, IDockerService dockerService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _dockerService = dockerService;
            _serviceProvider = serviceProvider;
            _serviceScope = _serviceProvider.CreateScope();
        }

        public Task ConsumeAsync(GetExecutionOutputMessage message) 
        {
            _logger.LogInformation("GetExecutionOutputMessage Received");

            ResourceDTO outputResource = _dockerService.GetExecutionOutputResource(message.PipelineExecutionId, message.ResourceId);

            // Publish ExecuteOperatorResultMessage to Orchestrator
            var getExecutionOutputResultMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetExecutionOutputResultMessage>>();

            var getExecutionOutputResultMessage = new GetExecutionOutputResultMessage
            {
                TicketId = message.TicketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OutputResource = outputResource
            };

            getExecutionOutputResultMessageProducer.PublishMessage(getExecutionOutputResultMessage);

            return Task.CompletedTask;
        }
    }
}
