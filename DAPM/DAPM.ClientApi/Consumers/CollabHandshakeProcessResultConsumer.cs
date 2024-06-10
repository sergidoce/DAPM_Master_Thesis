using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;
using DAPM.ClientApi.Services.Interfaces;

namespace DAPM.ClientApi.Consumers
{
    public class CollabHandshakeProcessResultConsumer : IQueueConsumer<CollabHandshakeProcessResult>
    {
        private ILogger<CollabHandshakeProcessResultConsumer> _logger;
        private ITicketService _ticketService;

        public CollabHandshakeProcessResultConsumer(ILogger<CollabHandshakeProcessResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(CollabHandshakeProcessResult message)
        {
            _logger.LogInformation("CollabHandshakeProcessResult received");

            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            JToken requestedPeerIdentityJson = JToken.FromObject(message.RequestedPeerIdentity, serializer);

            //Serialization
            result["requestedPeerIdentity"] = requestedPeerIdentityJson;
            result["succeeded"] = message.Succeeded;
            result["message"] = message.Message;


            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
