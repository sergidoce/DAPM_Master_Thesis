using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;

namespace DAPM.ClientApi.Consumers
{
    public class GetRolesResultConsumer : IQueueConsumer<GetRolesResultMessage>
    {
        private ILogger<GetRolesResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetRolesResultConsumer(ILogger<GetRolesResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetRolesResultMessage message)
        {
            _logger.LogInformation("GetRolesResultMessage received");


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
