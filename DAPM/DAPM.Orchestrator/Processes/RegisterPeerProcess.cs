using System.Xml.Linq;

namespace DAPM.Orchestrator.Processes
{
    public class RegisterPeerProcess : OrchestratorProcess
    {
        private string _introductionPeerAddress;
        private string _localPeerAddress;
        private string _peerName;

        public RegisterPeerProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, string introductionPeerAddress, string localPeerAddress, string peerName)
            : base(engine, serviceProvider, ticketId)
        {
            _introductionPeerAddress = introductionPeerAddress;
            _localPeerAddress = localPeerAddress;
            _peerName = peerName;
        }

        public override void StartProcess()
        {
            throw new NotImplementedException();
        }
    }
}
