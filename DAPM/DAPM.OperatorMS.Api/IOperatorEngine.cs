using RabbitMQLibrary.Messages.Operator;

namespace DAPM.OperatorMS.Api
{
    public interface IOperatorEngine
    {
        public void DeleteProcess(Guid processId);
        public OperatorProcess GetProcess(Guid processId);
        public void StartStoreFilesProcess(StoreFilesForExecutionMessage message);
        public void StartExecuteMinerProcess(ExecuteMinerMessage message);
    }
}
