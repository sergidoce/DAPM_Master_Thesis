using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.PeerApi
{
    public class SendRegistryUpdateAckMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public IdentityDTO SenderPeerIdentity { get; set; }
        public string TargetPeerDomain { get; set; }
        public RegistryUpdateAckDTO RegistryUpdateAck { get; set; }
    }
}
