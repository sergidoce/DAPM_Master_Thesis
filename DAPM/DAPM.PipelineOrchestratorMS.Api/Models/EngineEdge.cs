using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Models
{
    public class EngineEdge
    {
        private Guid _sourceNode;
        private Guid _targetNode;
        public EngineEdge(Guid sourceNode, Guid targetNode)
        {
            _sourceNode = sourceNode;
            _targetNode = targetNode;
        }

        public Guid GetSourceNode()
        {
            return _sourceNode;
        }

        public Guid GetTargetNode()
        {
            return _targetNode;
        }
    }
}
