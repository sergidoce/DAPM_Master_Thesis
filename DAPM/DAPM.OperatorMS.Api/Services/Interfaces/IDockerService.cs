using RabbitMQLibrary.Models;

namespace DAPM.OperatorMS.Api.Services.Interfaces
{
    public interface IDockerService
    {
        public void PostInputResource(Guid pipelineExecutionId, ResourceDTO inputResource);
        public void PostOperator(Guid pipelineExecutionId, ResourceDTO sourceCodeResource, FileDTO dockerfile);
        public ResourceDTO GetExecutionOutputResource(Guid pipelineExecutionId, Guid outputResourceId);
        public Task<string> CreateDockerImage(Guid pipelineExecutionId, Guid resourceId);
        public Task<string> CreateDockerContainerByImageName(string imageName);
        public Task<string> GetContainerStatus(string containerId);
        public Task ReplaceDockerfilePlaceholders(Guid pipelineExecutionId, Guid outputResourceId, Guid operatorId);
    }
}
