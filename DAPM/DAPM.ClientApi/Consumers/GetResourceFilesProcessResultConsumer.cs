using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Interfaces;

namespace DAPM.ClientApi.Consumers
{
    public class GetResourceFilesProcessResultConsumer : IQueueConsumer<GetResourceFilesProcessResult>
    {
        private ILogger<GetResourceFilesProcessResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetResourceFilesProcessResultConsumer(ILogger<GetResourceFilesProcessResultConsumer> logger,
            ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetResourceFilesProcessResult message)
        {
            _logger.LogInformation("GetResourceFilesProcessResult received");

            _logger.LogInformation($"FILE NAME {message.Files.First().Name}");
            IEnumerable<FileDTO> filesDTOs = message.Files;

            if(filesDTOs.Any())
            {
                var firstFile = filesDTOs.First();
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "TemporaryFiles");
                var filePath = Path.Combine(directoryPath, Path.GetRandomFileName());

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllBytes(filePath, firstFile.Content);

                JToken result = new JObject();
                //Serialization
                result["filePath"] = filePath;
                result["fileName"] = firstFile.Name;
                result["fileFormat"] = firstFile.Extension;


                // Update resolution
                _ticketService.UpdateTicketResolution(message.TicketId, result);

            }

            return Task.CompletedTask;
        }
    }
}
