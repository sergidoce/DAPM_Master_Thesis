using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

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
            string base64Image = Convert.ToBase64String(executionResult);

            JToken result = JToken.FromObject(new { Image = base64Image });

            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.FromResult(executionResult);
        }
    }
}
