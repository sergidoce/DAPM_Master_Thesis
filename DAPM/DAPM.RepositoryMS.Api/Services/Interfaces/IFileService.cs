using DAPM.RepositoryMS.Api.Models.MongoDB;

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IFileService
    {
        public Task<byte[]> GetFileContentById(string mongoId);
    }
}
