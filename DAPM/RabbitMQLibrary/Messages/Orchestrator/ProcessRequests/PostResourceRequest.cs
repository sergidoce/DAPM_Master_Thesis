﻿using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.Orchestrator.ProcessRequests
{
    public class PostResourceRequest : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public int OrganizationId { get; set; }
        public int RepositoryId { get; set; }
        public string Name { get; set; }
        public byte[] ResourceFile { get; set; }
    }
}
