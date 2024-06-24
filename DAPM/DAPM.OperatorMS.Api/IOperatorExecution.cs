using RabbitMQLibrary.Messages.Operator;
using RabbitMQLibrary.Models;

namespace DAPM.OperatorMS.Api
{
    public interface IOperatorExecution
    {
        public Task<bool> StartExecution();
        public void EndExecution();

    }
}
