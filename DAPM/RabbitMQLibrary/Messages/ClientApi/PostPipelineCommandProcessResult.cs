﻿using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Messages.ClientApi
{
    public class PostPipelineCommandProcessResult : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid ExecutionId { get; set; }
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }
}
