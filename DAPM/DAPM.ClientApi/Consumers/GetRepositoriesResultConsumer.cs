using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

namespace DAPM.ClientApi.Consumers
{
    public class GetRepositoriesResultConsumer : IQueueConsumer<GetRepositoriesResultMessage>
    {
        private ILogger<GetRepositoriesResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetRepositoriesResultConsumer(ILogger<GetRepositoriesResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetRepositoriesResultMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
