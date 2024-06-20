using DAPM.OperatorMS.Api.Processes;
using RabbitMQLibrary.Messages.Operator;

namespace DAPM.OperatorMS.Api
{
    public class OperatorEngine : IOperatorEngine
    {
        private Dictionary<Guid, OperatorProcess> _processes;
        private IServiceProvider _serviceProvider;

        public OperatorEngine(IServiceProvider serviceProvider) 
        {
            _processes = new Dictionary<Guid, OperatorProcess>();
            _serviceProvider = serviceProvider;
        }

        public void DeleteProcess(Guid processId) 
        {
            _processes.Remove(processId);
        }

        public OperatorProcess GetProcess(Guid processId)
        {
            return _processes[processId];
        }

        public void StartStoreFilesProcess(StoreFilesForExecutionMessage message) 
        {
            var storeFilesProcess = new StoreFilesProcess(this, _serviceProvider, message.TicketId, message);
            _processes[message.TicketId] = storeFilesProcess;
            storeFilesProcess.StartProcess();
        }

        public void StartExecuteMinerProcess(ExecuteMinerMessage message)
        {
            var executeMinerProcess = new ExecuteMinerProcess(this, _serviceProvider, message.TicketId, message);
            _processes[message.TicketId] = executeMinerProcess;
            executeMinerProcess.StartProcess();
        }
    }
}
