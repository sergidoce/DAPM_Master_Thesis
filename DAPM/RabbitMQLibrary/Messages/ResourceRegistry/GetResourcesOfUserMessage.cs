using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.ResourceRegistry
{
    public class GetResourcesOfUserMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
    }
}
