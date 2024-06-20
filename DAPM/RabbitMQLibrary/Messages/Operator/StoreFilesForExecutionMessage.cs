using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;

namespace RabbitMQLibrary.Messages.Operator
{
    public class StoreFilesForExecutionMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid TicketId { get; set; }
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public List<FileDTO> InputFiles { get; set; }
        public FileDTO SourceCode {  get; set; } 
        public FileDTO Dockerfile {  get; set; }
    }
}
