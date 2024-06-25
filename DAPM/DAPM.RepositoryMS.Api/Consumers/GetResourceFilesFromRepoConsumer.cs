using DAPM.RepositoryMS.Api.Models.MongoDB;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class GetResourceFilesFromRepoConsumer : IQueueConsumer<GetResourceFilesFromRepoMessage>
    {
        private ILogger<GetResourceFilesFromRepoConsumer> _logger;
        private IResourceService _resourceService;
        private IFileService _fileService;
        private IQueueProducer<GetResourceFilesFromRepoResultMessage> _resultMessageProducer;

        public GetResourceFilesFromRepoConsumer(ILogger<GetResourceFilesFromRepoConsumer> logger, 
            IResourceService resourceService, 
            IQueueProducer<GetResourceFilesFromRepoResultMessage> resultMessageProducer,
            IFileService fileService)
        {
            _logger = logger;
            _resourceService = resourceService;
            _resultMessageProducer = resultMessageProducer;
            _fileService = fileService;
        }

        public async Task ConsumeAsync(GetResourceFilesFromRepoMessage message)
        {
            _logger.LogInformation("GetResourceFilesFromRepoMessage received");

            var resourceWithoutFile = await _resourceService.GetResourceById(message.RepositoryId, message.ResourceId);

            var fileWithoutContent = await _resourceService.GetResourceFiles(message.RepositoryId, message.ResourceId);
            var filesDtos = new List<FileDTO>();

            
            var fileContent = await _fileService.GetFileContentById(fileWithoutContent.MongoDbFileId);
            var fileDto = new FileDTO()
            {
                Name = fileWithoutContent.Name,
                Extension = fileWithoutContent.Extension,
                Content = fileContent,

            };

            filesDtos.Add(fileDto);

            var resourceDTO = new ResourceDTO()
            {
                Id = resourceWithoutFile.Id,
                Name = resourceWithoutFile.Name,
                OrganizationId = message.OrganizationId,
                RepositoryId = resourceWithoutFile.RepositoryId,
                Type = resourceWithoutFile.Type,
                File = fileDto,
            };
         

            var resultMessage = new GetResourceFilesFromRepoResultMessage()
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Resource = resourceDTO
            };

            _resultMessageProducer.PublishMessage(resultMessage);

            _logger.LogInformation("GetResourceFilesFromRepoResultMessage Enqueued");

            return;
        }
    }
}
