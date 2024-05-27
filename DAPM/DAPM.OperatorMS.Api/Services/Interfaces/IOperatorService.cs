using Microsoft.AspNetCore.Mvc;

namespace DAPM.OperatorMS.Api.Services.Interfaces
{
    public interface IOperatorService
    {
        Task<byte[]> ExecuteMiner(IFormFile event_log, IFormFile dockerFile, IFormFile sourceCodeFile);
    }
}
