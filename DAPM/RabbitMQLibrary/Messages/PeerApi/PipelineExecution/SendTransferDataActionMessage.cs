using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.PeerApi.PipelineExecution
{
    public class SendTransferDataActionMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid SenderProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public string TargetPeerDomain { get; set; }
        public IdentityDTO SenderIdentity { get; set; }
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public TransferDataActionDTO Data { get; set; }
    }
}
