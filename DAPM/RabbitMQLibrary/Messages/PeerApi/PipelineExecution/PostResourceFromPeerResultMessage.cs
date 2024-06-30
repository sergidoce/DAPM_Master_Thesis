using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.PeerApi.PipelineExecution
{
    public class PostResourceFromPeerResultMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public string TargetPeerDomain { get; set; }
        public bool Succeeded { get; set; }
    }
}
