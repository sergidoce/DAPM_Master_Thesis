using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

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
            throw new NotImplementedException();
        }
    }
}
