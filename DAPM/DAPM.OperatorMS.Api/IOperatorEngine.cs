using RabbitMQLibrary.Messages.Operator;

namespace DAPM.OperatorMS.Api
{
    public interface IOperatorEngine
    {
        public void DeleteExecution(Guid processId);
        public OperatorExecution GetExecution(Guid processId);
        public Task<bool> StartOperatorExecution(ExecuteOperatorMessage message);
    }
}
