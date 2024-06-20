using RabbitMQLibrary.Messages.Operator;

namespace DAPM.OperatorMS.Api
{
    public interface IOperatorProcess
    {
        public void StartProcess();
        public void EndProcess();

    }
}
