using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Consumers
{
    public class GetOrganizationsResultConsumer : IQueueConsumer<GetOrganizationsResultMessage>
    {
        private ILogger<GetOrganizationsResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetOrganizationsResultConsumer(ILogger<GetOrganizationsResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetOrganizationsResultMessage message)
        {
            _logger.LogInformation("GetOrganizationsResultMessage received");


            IEnumerable<OrganizationDTO> organizationsDTOs = message.Organizations;

            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });


            //Serialization
            JToken organizationsJSON = JToken.FromObject(organizationsDTOs, serializer);
            result["organizations"] = organizationsJSON;


            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);
            
            return Task.CompletedTask;
        }

    }
}
