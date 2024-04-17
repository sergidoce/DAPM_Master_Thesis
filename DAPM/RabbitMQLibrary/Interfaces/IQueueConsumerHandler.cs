using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Interfaces
{
    public interface IQueueConsumerHandler<TMessageConsumer, TQueueMessage> where TMessageConsumer: IQueueConsumer<TQueueMessage> where TQueueMessage : class, IQueueMessage
    {
        void RegisterQueueConsumer();
        void CancelQueueConsumer();
    }
}
