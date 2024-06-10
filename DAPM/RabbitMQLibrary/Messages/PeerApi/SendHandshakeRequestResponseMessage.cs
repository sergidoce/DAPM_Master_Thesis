using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.PeerApi
{
    public class SendHandshakeRequestResponseMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public string TargetDomain { get; set; }
        public IdentityDTO SenderPeerIdentity { get; set; }
        public bool IsRequestAccepted { get; set; }
    }
}
