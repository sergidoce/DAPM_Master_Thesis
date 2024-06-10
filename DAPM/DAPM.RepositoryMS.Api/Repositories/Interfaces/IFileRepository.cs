using DAPM.RepositoryMS.Api.Models.MongoDB;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Task<string> AddFile(MongoFile file);
        Task<byte[]> GetFileContentById(string fileId);
    }
}
