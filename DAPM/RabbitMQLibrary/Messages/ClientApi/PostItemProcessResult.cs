using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.ClientApi
{
    public class ItemIds
    {
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid? ResourceId { get; set; }
        public Guid? PipelineId { get; set; }
        public Guid? ExecutionId { get; set; }
    }

    public class PostItemProcessResult : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public ItemIds ItemIds { get; set; }
        public string ItemType { get; set; }
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }
}
