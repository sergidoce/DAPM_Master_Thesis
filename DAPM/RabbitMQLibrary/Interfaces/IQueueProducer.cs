using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Interfaces
{
    public interface IQueueProducer<in TQueueMessage> where TQueueMessage : IQueueMessage
    {
        void PublishMessage(TQueueMessage message);
    }
}
