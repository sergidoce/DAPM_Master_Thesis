using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.PeerApi.PipelineExecution
{
    public class SendResourceToPeerMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid SenderProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid ExecutionId { get; set; }
        public IdentityDTO SenderPeerIdentity { get; set; }
        public string TargetPeerDomain { get; set; }
        public int StorageMode { get; set; }
        public Guid? RepositoryId { get; set; }
        public ResourceDTO Resource { get; set; }
    }
}
