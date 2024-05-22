using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using File = DAPM.RepositoryMS.Api.Models.PostgreSQL.File;
using DAPM.RepositoryMS.Api.Models;
using Microsoft.Extensions.Options;
using DAPM.RepositoryMS.Api.Models.MongoDB;

namespace DAPM.RepositoryMS.Api.Repositories
{
    public class FileRepository : IFileRepository
    {
        private ILogger<FileRepository> _logger;
        private readonly IMongoCollection<File> _fileCollection;
        private readonly IGridFSBucket _fileBucket;

        public FileRepository(ILogger<FileRepository> logger, IOptions<FileStorageDatabaseSettings> fileStorageDatabaseSettings)
        {
            _logger = logger;

            var mongoClient = new MongoClient(fileStorageDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(fileStorageDatabaseSettings.Value.DatabaseName);

            _fileCollection = mongoDatabase.GetCollection<File>(fileStorageDatabaseSettings.Value.FileCollectionName);
            _fileBucket = new GridFSBucket(mongoDatabase);
        }


        public async Task<string> AddFile(MongoFile file)
        {
            GridFSUploadOptions uploadOptions = new GridFSUploadOptions()
            {
                Metadata = new MongoDB.Bson.BsonDocument { { "type", "csv" }, { "owner", "me" } }
            };
            return (await _fileBucket.UploadFromBytesAsync(file.Name, file.File, uploadOptions)).ToString();
        }
    }
}
