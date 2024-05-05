using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

namespace DAPM.ClientApi.Consumers
{
    public class CreateNewItemResultConsumer : IQueueConsumer<CreateNewItemResultMessage>
    {
        private ILogger<CreateNewItemResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public CreateNewItemResultConsumer(ILogger<CreateNewItemResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(CreateNewItemResultMessage message)
        {
            _logger.LogInformation("CreateNewItemResultMessage received");


            // Objects used for serialization
            JToken result = new JObject();

            //Serialization
            result["itemId"] = message.ItemId;
            result["itemType"] = message.ItemType;
            result["succeeded"] = message.Succeeded;
            result["message"] = message.Message;  

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
