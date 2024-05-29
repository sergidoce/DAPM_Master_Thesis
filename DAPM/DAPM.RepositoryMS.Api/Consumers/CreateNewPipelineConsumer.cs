using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Repository;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class CreateNewPipelineConsumer : IQueueConsumer<CreateNewPipelineMessage>
    {
        private ILogger<CreateNewPipelineConsumer> _logger;
        private IRepositoryService _repositoryService;
        IQueueProducer<PostItemProcessResult> _createNewItemResultProducer;

        public CreateNewPipelineConsumer(ILogger<CreateNewPipelineConsumer> logger,
            IRepositoryService repositoryService,
            IQueueProducer<PostItemProcessResult> createNewItemResultProducer)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _createNewItemResultProducer = createNewItemResultProducer;
        }

        public async Task ConsumeAsync(CreateNewPipelineMessage message)
        {
            _logger.LogInformation("CreateNewResourceMessage received");

            int pipelineId = await _repositoryService.CreateNewPipeline(message.RepositoryId, message.Name, message.Pipeline);

            if(pipelineId != -1)
            {
                var resultMessage = new PostItemProcessResult
                {
                    TicketId = message.TicketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    ItemId = pipelineId,
                    ItemType = "Pipeline",
                    Message = "Item created successfully",
                    Succeeded = true,
                };

                _createNewItemResultProducer.PublishMessage(resultMessage);

                _logger.LogInformation("CreateNewItemResultMessage produced");

            }
            else
            {
                _logger.LogInformation("There was an error creating the new pipeline");
            }
        
            return;
        }
    }
}
