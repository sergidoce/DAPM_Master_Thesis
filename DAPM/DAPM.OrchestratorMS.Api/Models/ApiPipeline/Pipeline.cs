namespace DAPM.OrchestratorMS.Api.Models.Pipeline
{
    public class Pipeline
    {
        public IEnumerable<Node> Nodes { get; set; }
        public IEnumerable<Edge> Edges {  get; set; }
    }
}
