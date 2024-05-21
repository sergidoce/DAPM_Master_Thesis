using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Operator;
using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace DAPM.ClientApi.Consumers
{
    public class OperatorResultMessageConsumer : IQueueConsumer<OperatorResultMessage>
    {
        private ILogger<OperatorResultMessageConsumer> _logger;
        private readonly ITicketService _ticketService;

        public OperatorResultMessageConsumer(ILogger<OperatorResultMessageConsumer> logger, ITicketService ticketService) 
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(OperatorResultMessage message) 
        {
            _logger.LogInformation("OperatorResultMessage Received");

            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            JToken messageTextJSON = JToken.FromObject(message.MessageText, serializer);
            result["MessageText"] = messageTextJSON;

            _ticketService.UpdateTicketResolution(message.TicketId, result);
            return Task.CompletedTask;
        }
    }
}
