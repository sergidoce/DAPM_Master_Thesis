using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Interfaces;

namespace DAPM.ClientApi.Consumers
{
    public class LoginResultConsumer : IQueueConsumer<LoginResultMessage>
    {
        private ILogger<LoginResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public LoginResultConsumer(ILogger<LoginResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(LoginResultMessage message)
        {
            _logger.LogInformation("LoginResultMessage received");


            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            //Serialization
            result["succeeded"] = message.Succeeded;

            if (message.Succeeded)
            {
                result["message"] = JToken.Parse(message.Message);
            }
            else
            {
                result["message"] = message.Message;
            }
            

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
