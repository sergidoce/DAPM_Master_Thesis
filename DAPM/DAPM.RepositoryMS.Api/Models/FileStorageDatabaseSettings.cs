namespace DAPM.RepositoryMS.Api.Models
{
    public class FileStorageDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string FileCollectionName { get; set; } = null!;
    }
}
