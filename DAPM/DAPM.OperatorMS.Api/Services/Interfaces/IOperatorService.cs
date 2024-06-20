

using RabbitMQLibrary.Models;

namespace DAPM.OperatorMS.Api.Services.Interfaces
{
    public interface IOperatorService
    {
        Task<FileDTO> ExecuteMiner(Guid executionId, Guid stepId);
    }
}
