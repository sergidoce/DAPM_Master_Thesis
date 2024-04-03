namespace DAPM.RepositoryMS.Api.Models
{
    public class ResourceDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ResourcesCollectionName { get; set; } = null!;
    }
}
