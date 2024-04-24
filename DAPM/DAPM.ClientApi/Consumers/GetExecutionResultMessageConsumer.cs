using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages;

namespace DAPM.ClientApi.Consumers
{
    public class GetExecutionResultMessageConsumer : IQueueConsumer<GetExecutionResultMessage>
    {
        private ILogger<GetExecutionResultMessageConsumer> _logger;
        private readonly ITicketService _ticketService;

        public GetExecutionResultMessageConsumer(ILogger<GetExecutionResultMessageConsumer> logger, ITicketService ticketService) { 
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetExecutionResultMessage message) {
            _logger.LogInformation("Message received");

            byte[]? executionResult = message.ExecutionResult;

            return Task.FromResult(executionResult);
        }
    }
}
