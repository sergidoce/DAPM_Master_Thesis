using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class GetOperatorFilesFromRepoConsumer : IQueueConsumer<GetOperatorFilesFromRepoMessage>
    {
        private ILogger<GetOperatorFilesFromRepoConsumer> _logger;
        private IResourceService _resourceService;
        private IFileService _fileService;
        private IQueueProducer<GetOperatorFilesFromRepoResultMessage> _resultMessageProducer;

        public GetOperatorFilesFromRepoConsumer(ILogger<GetOperatorFilesFromRepoConsumer> logger,
            IResourceService resourceService,
            IQueueProducer<GetOperatorFilesFromRepoResultMessage> resultMessageProducer,
            IFileService fileService)
        {
            _logger = logger;
            _resourceService = resourceService;
            _resultMessageProducer = resultMessageProducer;
            _fileService = fileService;
        }
        public async Task ConsumeAsync(GetOperatorFilesFromRepoMessage message)
        {
            _logger.LogInformation("GetOperatorFilesFromRepoMessage received");

            var resourceWithoutFile = await _resourceService.GetOperatorById(message.RepositoryId, message.ResourceId);

            (var sourceCodeFileWithoutContent, var dockerfileFileWithoutContent) = 
                await _resourceService.GetOperatorFiles(message.RepositoryId, message.ResourceId);

            var sourceCodeFileContent = await _fileService.GetFileContentById(sourceCodeFileWithoutContent.MongoDbFileId);
            var dockerfileFileContent = await _fileService.GetFileContentById(dockerfileFileWithoutContent.MongoDbFileId);

            var sourceCodeFileDto = new FileDTO()
            {
                Name = sourceCodeFileWithoutContent.Name,
                Extension = sourceCodeFileWithoutContent.Extension,
                Content = sourceCodeFileContent,

            };

            var dockerFileDto = new FileDTO()
            {
                Name = dockerfileFileWithoutContent.Name,
                Extension = dockerfileFileWithoutContent.Extension,
                Content = dockerfileFileContent,
            };

            var resourceDTO = new ResourceDTO()
            {
                Id = resourceWithoutFile.Id,
                Name = resourceWithoutFile.Name,
                OrganizationId = message.OrganizationId,
                RepositoryId = resourceWithoutFile.RepositoryId,
                Type = resourceWithoutFile.Type,
                File = sourceCodeFileDto,
            };


            var resultMessage = new GetOperatorFilesFromRepoResultMessage()
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                SourceCodeResource = resourceDTO,
                DockerfileFile = dockerFileDto,
            };

            _resultMessageProducer.PublishMessage(resultMessage);

            _logger.LogInformation("GetOperatorFilesFromRepoResultMessage Enqueued");

            return;
        }
    }
}
