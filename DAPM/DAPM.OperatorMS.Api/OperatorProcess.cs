
namespace DAPM.OperatorMS.Api
{
    public abstract class OperatorProcess : IOperatorProcess
    {
        private IServiceProvider _serviceProvider;
        protected IServiceScope _serviceScope;
        protected Guid _ticketId;
        protected OperatorEngine _engine;

        public OperatorProcess(OperatorEngine engine, IServiceProvider serviceProvider, Guid ticketId)
        {
            _engine = engine;
            _serviceProvider = serviceProvider;
            _ticketId = ticketId;
            _serviceScope = _serviceProvider.CreateScope();
        }

        public abstract void StartProcess();

        public virtual void EndProcess()
        {
            _engine.DeleteProcess(_ticketId);
        }
    }
}
