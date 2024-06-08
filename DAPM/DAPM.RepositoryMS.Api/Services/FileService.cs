using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;

namespace DAPM.RepositoryMS.Api.Services
{
    public class FileService : IFileService
    {
        private IFileRepository _fileRepository;
        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public async Task<byte[]> GetFileContentById(string mongoId)
        {
            return await _fileRepository.GetFileContentById(mongoId);
        }
    }
}
