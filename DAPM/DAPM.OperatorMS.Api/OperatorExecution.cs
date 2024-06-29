
using DAPM.OperatorMS.Api.Services.Interfaces;
using RabbitMQLibrary.Models;

namespace DAPM.OperatorMS.Api
{
    public class OperatorExecution : IOperatorExecution
    {
        protected Guid _ticketId;
        protected IOperatorEngine _engine;
        private IDockerService _dockerService;
        private Guid _pipelineExecutionId;
        private Guid _outputResourceId;
        private ResourceDTO _sourceCode;
        private FileDTO _dockerfile;

        public OperatorExecution(IOperatorEngine engine, Guid ticketId, Guid pipelineExecutionId, Guid outputResourceId,
            ResourceDTO sourceCode, FileDTO dockerfile, IDockerService dockerService)
        {
            _engine = engine;
            _ticketId = ticketId;
            _dockerService = dockerService;
            _pipelineExecutionId = pipelineExecutionId;
            _outputResourceId = outputResourceId;
            _sourceCode = sourceCode;
            _dockerfile = dockerfile;

        }

        public async Task<bool> StartExecution()
        {
            // Post operator (source-code and Dockerfile) to Docker Volume
            _dockerService.PostOperator(_pipelineExecutionId, _sourceCode, _dockerfile);

            // Replace placeholders in Dockerfile with corresponding input file path and output file path
            await _dockerService.ReplaceDockerfilePlaceholders(_pipelineExecutionId, _outputResourceId, _sourceCode.Id);

            // Create Docker image with source-code and Dockerfile
            string imageName = await _dockerService.CreateDockerImage(_pipelineExecutionId, _sourceCode.Id);

            // Create and start Docker container from Docker image 
            string containerId = await _dockerService.CreateDockerContainerByImageName(imageName);

            // Wait until Docker container has stopped
            string containerStatus;
            do
            {
                containerStatus = await _dockerService.GetContainerStatus(containerId);
                await Task.Delay(TimeSpan.FromSeconds(1));
            } while (containerStatus.ToLower() == "running");

            // Check whether the expected output file is placed in the Docker volume
            ResourceDTO outputFile = _dockerService.GetExecutionOutputResource(_pipelineExecutionId, _outputResourceId);
            bool succeeded = outputFile != null;

            return succeeded;
        }

        public void EndExecution()
        {
            _engine.DeleteExecution(_ticketId);
        }
    }
}
