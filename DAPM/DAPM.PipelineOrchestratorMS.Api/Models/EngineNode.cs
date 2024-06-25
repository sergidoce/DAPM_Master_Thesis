using RabbitMQLibrary.Models;

namespace DAPM.PipelineOrchestratorMS.Api.Models
{
    public class EngineNode
    {
        public Guid Id { get; set; }
        public string NodeType { get; set; }
        public List<string> SourceHandles { get; set; }
        public List<string> TargetHandles { get; set; }

        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid? ResourceId { get; set; }
        public string? ResourceName { get; set; }

        private List<Guid> _associatedSteps { get; set; }

        public EngineNode(Node node)
        {
            Id = Guid.NewGuid();
            _associatedSteps = new List<Guid>();
            SourceHandles = new List<string>();
            TargetHandles = new List<string>();

            NodeType = node.Type;

            foreach(var sourceHandle in node.TemplateData.SourceHandles)
            {
                SourceHandles.Add(sourceHandle.HandleData.Id);
            }

            foreach (var targetHandle in node.TemplateData.TargetHandles)
            {
                TargetHandles.Add(targetHandle.HandleData.Id);
            }

            OrganizationId = node.InstantiationData.Resource.OrganizationId;
            RepositoryId = node.InstantiationData.Resource.RepositoryId;

            if (NodeType != "dataSink")
                ResourceId = node.InstantiationData.Resource.ResourceId;
            else
            {
                ResourceId = null;
                ResourceName = node.InstantiationData.Resource.Name;
            }
                
        }
        
        public void AddAssociatedStep(Guid id)
        {
            _associatedSteps.Add(id);
        }

        public List<Guid> GetAssociatedStep(Guid id)
        {
            return _associatedSteps;
        }

        public Guid GetLastAssociatedStep()
        {
            if (_associatedSteps.Count == 0)
                return Guid.Empty;

            return _associatedSteps.ElementAt(_associatedSteps.Count - 1);
        }
    }
}
