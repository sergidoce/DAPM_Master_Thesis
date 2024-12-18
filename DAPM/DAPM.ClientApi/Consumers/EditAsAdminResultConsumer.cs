using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;

namespace DAPM.ClientApi.Consumers
{
    public class EditAsAdminResultConsumer : IQueueConsumer<EditAsAdminResultMessage>
    {
        private ILogger<EditAsAdminResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public EditAsAdminResultConsumer(ILogger<EditAsAdminResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(EditAsAdminResultMessage message)
        {
            _logger.LogInformation("EditAsAdminResultMessage received");


            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            //Serialization
            result["succeeded"] = message.Succeeded;
            result["message"] = message.Message;

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
