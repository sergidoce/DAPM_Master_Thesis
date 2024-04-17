using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Consumers
{
    public class GetOrganisationsResultMessageConsumer : IQueueConsumer<GetOrganisationsResultMessage>
    {
        private ILogger<GetOrganisationsResultMessageConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetOrganisationsResultMessageConsumer(ILogger<GetOrganisationsResultMessageConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetOrganisationsResultMessage message)
        {
            _logger.LogInformation("Message received");


            Organisation[] organisations = message.Organisations;

            _logger.LogInformation("It just works");

            return Task.CompletedTask;
        }

    }
}
