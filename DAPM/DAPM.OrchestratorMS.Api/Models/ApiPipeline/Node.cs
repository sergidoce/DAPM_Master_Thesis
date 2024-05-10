namespace DAPM.OrchestratorMS.Api.Models.Pipeline
{
    public class Node
    {
        public string Type { get; set; }
        public TemplateData TemplateData { get; set; }
        public InstantiationData InstantiationData { get; set; }
    }
}
