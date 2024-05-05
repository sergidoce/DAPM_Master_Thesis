using DAPM.OperatorMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

namespace DAPM.OperatorMS.Api.Consumers
{
    public class GetExecutionMessageConsumer : IQueueConsumer<GetExecutionMessage>
    {
        private ILogger<GetExecutionMessageConsumer> _logger;
        private IQueueProducer<GetExecutionResultMessage> _getExecutionResultMessageQueueProducer;
        private IOperatorService _operatorService;

        public GetExecutionMessageConsumer(ILogger<GetExecutionMessageConsumer> logger, IQueueProducer<GetExecutionResultMessage> getExecutionResultMessage, IOperatorService operatorService)
        {
            _logger = logger;
            _getExecutionResultMessageQueueProducer = getExecutionResultMessage;
            _operatorService = operatorService;
        }

        public async Task ConsumeAsync(GetExecutionMessage message)
        {
            _logger.LogInformation("Get ExecutionMessage received");

            byte[] pngImageBytes = await _operatorService.ExecuteMiner(message.MinerId, message.ResourceId);

            var result_message = new GetExecutionResultMessage {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                ExecutionResult = pngImageBytes
            };

            _getExecutionResultMessageQueueProducer.PublishMessage(result_message);
        }
    }
}
