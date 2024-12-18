using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;

namespace DAPM.ClientApi.Consumers
{
    public class AddRolesResultConsumer : IQueueConsumer<AddRolesResultMessage>
    {
        private ILogger<AddRolesResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public AddRolesResultConsumer(ILogger<AddRolesResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(AddRolesResultMessage message)
        {
            _logger.LogInformation("AddRolesResultMessage received");


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
