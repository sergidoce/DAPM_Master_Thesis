using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class PostPipelineToRepoConsumer : IQueueConsumer<PostPipelineToRepoMessage>
    {
        private ILogger<PostPipelineToRepoConsumer> _logger;
        private IRepositoryService _repositoryService;
        IQueueProducer<PostPipelineToRepoResultMessage> _postPipelineToRepoResultProducer;

        public PostPipelineToRepoConsumer(ILogger<PostPipelineToRepoConsumer> logger,
            IRepositoryService repositoryService,
            IQueueProducer<PostPipelineToRepoResultMessage> postPipelineToRepoResultProducer)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _postPipelineToRepoResultProducer = postPipelineToRepoResultProducer;
        }

        public async Task ConsumeAsync(PostPipelineToRepoMessage message)
        {
            _logger.LogInformation("PostPipelineToRepoMessage received");

            Models.PostgreSQL.Pipeline pipeline = await _repositoryService.CreateNewPipeline(message.RepositoryId, message.Name, message.Pipeline);

            if(pipeline != null)
            {
                var pipelineDTO = new PipelineDTO()
                {
                    Id = pipeline.Id,
                    RepositoryId = pipeline.RepositoryId,
                    Name = pipeline.Name,
                    Pipeline = JsonConvert.DeserializeObject<RabbitMQLibrary.Models.Pipeline>(pipeline.PipelineJson)
                };

                var resultMessage = new PostPipelineToRepoResultMessage
                {
                    TicketId = message.TicketId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Message = "Item created successfully",
                    Succeeded = true,
                    Pipeline = pipelineDTO
                };

                _postPipelineToRepoResultProducer.PublishMessage(resultMessage);

                _logger.LogInformation("PostPipelineToRepoResultMessage produced");

            }
            else
            {
                _logger.LogInformation("There was an error creating the new pipeline");
            }
        
            return;
        }
    }
}
