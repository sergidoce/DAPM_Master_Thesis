using Microsoft.AspNetCore.Mvc;

namespace DAPM.OperatorMS.Api.Services.Interfaces
{
    public interface IOperatorService
    {
        Task<string> ExecuteMiner(IFormFile eventlog, IFormFile sourceCode, IFormFile dockerFile);
    }
}
