using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class GetPipelinesConsumer : IQueueConsumer<GetPipelinesMessage>
    {
        private ILogger<GetPipelinesConsumer> _logger;
        private IQueueProducer<GetPipelinesResultMessage> _getPipelinesResultQueueProducer;
        private IRepositoryService _repositoryService;
        private IPipelineService _pipelineService;
        public GetPipelinesConsumer(ILogger<GetPipelinesConsumer> logger,
            IQueueProducer<GetPipelinesResultMessage> getPipelinesResultQueueProducer,
            IRepositoryService repositoryService,
            IPipelineService pipelineService)
        {
            _logger = logger;
            _getPipelinesResultQueueProducer = getPipelinesResultQueueProducer;
            _repositoryService = repositoryService;
            _pipelineService = pipelineService;
        }

        public async Task ConsumeAsync(GetPipelinesMessage message)
        {
            _logger.LogInformation("GetPipelinesMessage received");

            var pipelines = Enumerable.Empty<Models.Pipeline>();

            if (message.PipelineId != null)
            {
                var pipeline = await _pipelineService.GetPipelineById(message.OrganizationId, message.RepositoryId, (Guid)message.PipelineId);
                pipelines = pipelines.Append(pipeline);
            }
            else
            {
                pipelines = await _repositoryService.GetPipelinesOfRepository(message.OrganizationId, message.RepositoryId);
            }

            IEnumerable<PipelineDTO> pipelinesDTOs = Enumerable.Empty<PipelineDTO>();

            foreach (var pipeline in pipelines)
            {
                var r = new PipelineDTO
                {
                    Id = pipeline.Id,
                    Name = pipeline.Name,
                    OrganizationId = pipeline.PeerId,
                    RepositoryId = pipeline.RepositoryId,
                };

                pipelinesDTOs = pipelinesDTOs.Append(r);
            }

            var resultMessage = new GetPipelinesResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Pipelines = pipelinesDTOs
            };

            _getPipelinesResultQueueProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
