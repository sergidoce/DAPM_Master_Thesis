using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Consumers
{
    public class GetOrganisationsResultMessageConsumer : IQueueConsumer<GetOrganizationsResultMessage>
    {
        private ILogger<GetOrganisationsResultMessageConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetOrganisationsResultMessageConsumer(ILogger<GetOrganisationsResultMessageConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetOrganizationsResultMessage message)
        {
            _logger.LogInformation("Message received");

            OrganizationDTO[] organisations = message.Organisations;
            JToken result = new JObject();
            JToken organisationsJSON = JToken.FromObject(organisations);
            result["organisations"] = organisationsJSON;
            _ticketService.UpdateTicketResolution(message.TicketId, result);
            
            _logger.LogInformation("It just works");

            return Task.CompletedTask;
        }

    }
}
