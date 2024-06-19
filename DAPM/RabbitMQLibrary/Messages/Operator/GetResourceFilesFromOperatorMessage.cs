using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.Operator
{
    public class GetResourceFilesFromOperatorMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public Guid ResourceId { get; set; }
    }
}
