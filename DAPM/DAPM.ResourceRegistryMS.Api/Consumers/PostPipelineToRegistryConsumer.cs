using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class PostPipelineToRegistryConsumer : IQueueConsumer<PostPipelineToRegistryMessage>
    {
        private ILogger<PostPipelineToRegistryConsumer> _logger;
        private IRepositoryService _repositoryService;
        private IQueueProducer<PostPipelineToRegistryResultMessage> _postPipelineToRegistryResultProducer;
        public PostPipelineToRegistryConsumer(ILogger<PostPipelineToRegistryConsumer> logger,
            IQueueProducer<PostPipelineToRegistryResultMessage> postPipelineToRegistryResultProducer,
            IRepositoryService repositoryService)
        {
            _logger = logger;
            _postPipelineToRegistryResultProducer = postPipelineToRegistryResultProducer;
            _repositoryService = repositoryService;
        }
        public async Task ConsumeAsync(PostPipelineToRegistryMessage message)
        {
            _logger.LogInformation("PostPipelineToRegistryMessage received");

            var pipelineDto = message.Pipeline;
            if (pipelineDto != null)
            {
                var createdPipeline = _repositoryService.AddPipelineToRepository(pipelineDto.OrganizationId, pipelineDto.RepositoryId, pipelineDto);
                if (createdPipeline != null)
                {
                    var resultMessage = new PostPipelineToRegistryResultMessage
                    {
                        TicketId = message.TicketId,
                        TimeToLive = TimeSpan.FromMinutes(1),
                        Message = "Item created successfully",
                        Succeeded = true,
                        Pipeline = pipelineDto
                    };

                    _postPipelineToRegistryResultProducer.PublishMessage(resultMessage);
                    _logger.LogInformation("PostPipelineToRegistryResultMessage published");
                }
            }

            return;
        }
    }
}
