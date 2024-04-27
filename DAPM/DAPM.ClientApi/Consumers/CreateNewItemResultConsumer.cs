using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

namespace DAPM.ClientApi.Consumers
{
    public class CreateNewItemResultConsumer : IQueueConsumer<CreateNewItemResultMessage>
    {
        private ILogger<CreateNewItemResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public CreateNewItemResultConsumer(ILogger<CreateNewItemResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(CreateNewItemResultMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
