using DAPM.OperatorMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Models;

namespace DAPM.OperatorMS.Api.Consumers
{
    public class PostInputResourceMessageConsumer : IQueueConsumer<PostInputResourceMessage>
    {
        private ILogger<PostInputResourceMessageConsumer> _logger;
        private IDockerService _dockerService;
        private IServiceProvider _serviceProvider;
        protected IServiceScope _serviceScope;

        public PostInputResourceMessageConsumer(ILogger<PostInputResourceMessageConsumer> logger, IDockerService dockerService, IServiceProvider serviceProvider) 
        {
            _logger = logger;
            _dockerService = dockerService;
            _serviceProvider = serviceProvider;
            _serviceScope = _serviceProvider.CreateScope();
        }

        public Task ConsumeAsync(PostInputResourceMessage message) 
        {
            _logger.LogInformation("PostInputResourceMessage Received");

            
            _dockerService.PostInputResource(message.PipelineExecutionId, message.Resource);
            

            // Publish PostInputResourceResultMessage to Orchestrator
            var postInputResourceResultMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostInputResourceResultMessage>>();

            var postInputResourceResultMessage = new PostInputResourceResultMessage
            {
                TicketId = message.TicketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Succeeded = true,
            };

            postInputResourceResultMessageProducer.PublishMessage(postInputResourceResultMessage);

            return Task.CompletedTask;
        }
    }
}
