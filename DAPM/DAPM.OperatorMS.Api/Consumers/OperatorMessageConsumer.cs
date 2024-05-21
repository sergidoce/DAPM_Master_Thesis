using RabbitMQLibrary.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;
using DAPM.OperatorMS.Api.Services.Interfaces;

namespace DAPM.OperatorMS.Api.Consumers
{
    public class OperatorMessageConsumer : IQueueConsumer<OperatorMessage>
    {
        private ILogger<OperatorMessageConsumer> _logger;
        private IQueueProducer<OperatorResultMessage> _operatorResultMessageProducer;

        public OperatorMessageConsumer(ILogger<OperatorMessageConsumer> logger, IQueueProducer<OperatorResultMessage> operatorResultMessageProducer)
        {
            _logger = logger;
            _operatorResultMessageProducer = operatorResultMessageProducer;
        }

        public async Task ConsumeAsync(OperatorMessage message)
        {
            _logger.LogInformation("OperatorMessage Received");
            var resultMessage = new OperatorResultMessage 
            {
                TicketId = message.TicketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                MessageText = message.MessageText,
            };

            _operatorResultMessageProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
