using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using UtilLibrary;

namespace DAPM.ClientApi.Consumers
{
    public class GetUsersResultConsumer : IQueueConsumer<GetUsersResultMessage>
    {
        private ILogger<GetUsersResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetUsersResultConsumer(ILogger<GetUsersResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetUsersResultMessage message)
        {
            _logger.LogInformation("GetUsersResultMessage received");


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
