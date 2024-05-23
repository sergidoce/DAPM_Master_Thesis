using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;

namespace DAPM.OperatorMS.Api.Consumers
{
    public class RunAlgorithmResultMessageConsumer : IQueueConsumer<RunAlgorithmResultMessage>
    {
        private ILogger<RunAlgorithmResultMessageConsumer> _logger;

        public RunAlgorithmResultMessageConsumer(ILogger<RunAlgorithmResultMessageConsumer> logger) 
        {
            _logger = logger;
        }

        public async Task ConsumeAsync(RunAlgorithmResultMessage message)
        {
            _logger.LogInformation("RunAlgorithResultMessage Received!!!");
            _logger.LogInformation(message.Result);
        }
    }
}
