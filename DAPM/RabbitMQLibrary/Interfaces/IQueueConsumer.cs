using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Interfaces
{
    public interface IQueueConsumer <in TQueueMessage> where TQueueMessage : class, IQueueMessage
    {
        Task ConsumeAsync(TQueueMessage message);
    }
}
