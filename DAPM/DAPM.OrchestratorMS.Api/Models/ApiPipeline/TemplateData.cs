namespace DAPM.OrchestratorMS.Api.Models.Pipeline
{
    public class TemplateData
    {
        public IEnumerable<SourceHandle> SourceHandles { get; set; }
        public IEnumerable<TargetHandle> TargetHandles { get; set; }
    }
}
