namespace DAPM.OrchestratorMS.Api.Models.Pipeline
{
    public class Resource
    {
        public int OrganizationId { get; set; }
        public int RepositoryId { get; set; }
        public int? ResourceId { get; set; }
        public string? Name { get; set; }
    }
}
