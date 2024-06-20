using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Operator;

namespace DAPM.OperatorMS.Api.Consumers
{
    public class StoreFilesForExecutionConsumer : IQueueConsumer<StoreFilesForExecutionMessage>
    {
        private ILogger<StoreFilesForExecutionConsumer> _logger;
        private IOperatorEngine _operatorEngine;

        public StoreFilesForExecutionConsumer(ILogger<StoreFilesForExecutionConsumer> logger, IOperatorEngine operatorEngine) 
        {
            _logger = logger;
            _operatorEngine = operatorEngine;
        }

        public Task ConsumeAsync(StoreFilesForExecutionMessage message)
        {
            _logger.LogInformation("StoreFilesForExecutionMessage received");

            _operatorEngine.StartStoreFilesProcess(message);

            return Task.CompletedTask;
        }
    }
}
