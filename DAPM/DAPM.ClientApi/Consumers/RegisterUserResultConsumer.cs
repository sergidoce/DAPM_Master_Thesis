using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Interfaces;

namespace DAPM.ClientApi.Consumers
{
    public class RegisterUserResultConsumer : IQueueConsumer<RegisterUserResultMessage>
    {
        private ILogger<RegisterUserResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public RegisterUserResultConsumer(ILogger<RegisterUserResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(RegisterUserResultMessage message)
        {
            _logger.LogInformation("RegisterUserResultMessage received");


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
