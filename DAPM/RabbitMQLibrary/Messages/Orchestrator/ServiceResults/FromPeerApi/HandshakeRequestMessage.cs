using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi
{
    public class HandshakeRequestMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid SenderProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public IdentityDTO SenderPeerIdentity { get; set; }
    }
}
