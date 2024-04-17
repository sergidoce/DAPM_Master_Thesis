using Microsoft.AspNetCore.Mvc;

namespace DAPM.OperatorMS.Api.Services.Interfaces
{
    public interface IOperatorService
    {
        Task<byte[]> ExecuteMiner(string operatorName, string parameter);
    }
}
