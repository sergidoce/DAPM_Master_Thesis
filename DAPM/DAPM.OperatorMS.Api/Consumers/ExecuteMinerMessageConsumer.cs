using DAPM.OperatorMS.Api.Services;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;

namespace DAPM.OperatorMS.Api.Consumers
{
    public class ExecuteMinerMessageConsumer : IQueueConsumer<ExecuteMinerMessage>
    {
        private ILogger<ExecuteMinerMessageConsumer> _logger;
        private IOperatorEngine _operatorEngine;

        public ExecuteMinerMessageConsumer(ILogger<ExecuteMinerMessageConsumer> logger, IOperatorEngine operatorEngine) 
        {
            _logger = logger;
            _operatorEngine = operatorEngine;
        }

        public Task ConsumeAsync(ExecuteMinerMessage message) 
        {
            _logger.LogInformation("ExecuteMinerMessage Received");

            _operatorEngine.StartExecuteMinerProcess(message);

            return Task.CompletedTask;
        }
    }
}
