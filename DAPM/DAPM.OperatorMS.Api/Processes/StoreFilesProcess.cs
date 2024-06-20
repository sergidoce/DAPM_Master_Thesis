using DAPM.OperatorMS.Api.Services;
using RabbitMQLibrary.Messages.Operator;
using RabbitMQLibrary.Models;

namespace DAPM.OperatorMS.Api.Processes
{
    public class StoreFilesProcess : OperatorProcess
    {
        private StoreFilesForExecutionMessage _message;

        public StoreFilesProcess(OperatorEngine engine, IServiceProvider serviceProvider, Guid ticketId, StoreFilesForExecutionMessage message) : base(engine, serviceProvider, ticketId)
        {
            _message = message;
        }

        public override void StartProcess()
        {
            // Create folder for storing files for an execution in the Docker Volume
            // Folder is named using ExecutionId and StepId
            var volumeDirectoryPath = $"/app/shared/{_message.ExecutionId}_{_message.StepId}";
            Directory.CreateDirectory(volumeDirectoryPath);

            // Check whether there are more than one unput file, then store input file(s) into the created folder in Docker Volume
            if (_message.InputFiles.Count == 1)
            {
                var filePath = $"{volumeDirectoryPath}/input_{_message.ExecutionId}_{_message.StepId}{_message.InputFiles[0].Extension}";
                File.WriteAllBytes(filePath, _message.InputFiles[0].Content);
            }
            else
            {
                for (int i = 0; i < _message.InputFiles.Count; i++)
                {
                    var filePath = $"{volumeDirectoryPath}/input_{_message.ExecutionId}_{_message.StepId}_{i+1}{_message.InputFiles[i].Extension}";
                    File.WriteAllBytes(filePath, _message.InputFiles[i].Content);

                }
            }

            // Store source code in the Docker Volume
            var sourceCodeFilePath = $"{volumeDirectoryPath}/sourceCode_{_message.ExecutionId}_{_message.StepId}{_message.SourceCode.Extension}";
            File.WriteAllBytes(sourceCodeFilePath, _message.SourceCode.Content);

            // Store Dockerfile in the Docker Volume
            var dockerfilePath = $"{volumeDirectoryPath}/Dockerfile";
            File.WriteAllBytes(dockerfilePath, _message.Dockerfile.Content);

            EndProcess();
        }
    }
}
