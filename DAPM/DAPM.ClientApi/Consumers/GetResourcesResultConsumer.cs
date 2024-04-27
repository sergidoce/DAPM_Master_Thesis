using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

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
            throw new NotImplementedException();
        }
    }
}
