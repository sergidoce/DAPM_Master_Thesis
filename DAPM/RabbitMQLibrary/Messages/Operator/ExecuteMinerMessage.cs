using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.Operator
{
    public class ExecuteMinerMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public string OutputName { get; set; }
        public string ResourceType { get; set; }
    }
}
