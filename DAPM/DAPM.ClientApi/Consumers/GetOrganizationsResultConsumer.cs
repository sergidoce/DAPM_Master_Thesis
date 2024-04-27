using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            _logger.LogInformation("Message received");

            OrganizationDTO[] organisations = message.Organizations;
            JToken result = new JObject();
            JToken organisationsJSON = JToken.FromObject(organisations);
            result["organisations"] = organisationsJSON;
            _ticketService.UpdateTicketResolution(message.TicketId, result);
            
            _logger.LogInformation("It just works");

            return Task.CompletedTask;
        }

    }
}
