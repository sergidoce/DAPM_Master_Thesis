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
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public string Name { get; set; }
        public Pipeline Pipeline { get; set; }
    }
}
