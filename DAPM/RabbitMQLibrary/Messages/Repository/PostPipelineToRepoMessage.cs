using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.Repository
{
    public class PostPipelineToRepoMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public int OrganizationId { get; set; }
        public int RepositoryId { get; set; }
        public string Name { get; set; }
        public Pipeline Pipeline { get; set; }
    }
}
