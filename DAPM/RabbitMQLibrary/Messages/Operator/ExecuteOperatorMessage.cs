using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;

namespace RabbitMQLibrary.Messages.Operator
{
    public class ExecuteOperatorMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid PipelineExecutionId { get; set; }
        public Guid OutputResourceId { get; set; }
        public ResourceDTO SourceCode { get; set; }
        public FileDTO Dockerfile { get; set; }
    }
}
