using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using DAPM.RepositoryMS.Api.Models;

namespace DAPM.RepositoryMS.Api.Repositories
{
    public class ResourceRepository : IResourceRepository
    {

        private ILogger<ResourceRepository> _logger;
        private readonly IMongoCollection<Resource> _resourceCollection;
        private readonly IGridFSBucket _resourceBucket;

        public ResourceRepository(ILogger<ResourceRepository> logger, IOptions<ResourceDatabaseSettings> resourceDatabaseSettings)
        {
            _logger = logger;

            var mongoClient = new MongoClient(resourceDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(resourceDatabaseSettings.Value.DatabaseName);

            _resourceCollection = mongoDatabase.GetCollection<Resource>(resourceDatabaseSettings.Value.ResourcesCollectionName);
            _resourceBucket = new GridFSBucket(mongoDatabase);
        }

        public async Task<bool> AddResource(Resource resource)
        {
            resource.File.Position = 0;
            GridFSUploadOptions uploadOptions = new GridFSUploadOptions()
            {
                Metadata = new MongoDB.Bson.BsonDocument { { "type", "csv" }, { "owner", "me" } }
            };
            var id = await _resourceBucket.UploadFromStreamAsync(resource.Name, resource.File, uploadOptions);
            _logger.LogWarning("Hello from the repository");
            return true;
        }

        public async Task<Resource> RetrieveResource(string resourceName)
        {
            var filePath = Path.GetRandomFileName();
            var fileStream = new FileStream(filePath, FileMode.Create);
            await _resourceBucket.DownloadToStreamByNameAsync(resourceName, fileStream);

            return new Resource(resourceName, fileStream);
        }
    }
}
