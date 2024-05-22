using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Consumers
{
    public class GetResourcesResultConsumer : IQueueConsumer<GetResourcesResultMessage>
    {
        private ILogger<GetResourcesResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetResourcesResultConsumer(ILogger<GetResourcesResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetResourcesResultMessage message)
        {
            _logger.LogInformation("GetResourcesResultMessage received");


            IEnumerable<ResourceDTO> resourcesDTOs = message.Resources;

            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });


            //Serialization
            JToken resourcesJSON = JToken.FromObject(resourcesDTOs, serializer);
            result["resources"] = resourcesJSON;


            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
