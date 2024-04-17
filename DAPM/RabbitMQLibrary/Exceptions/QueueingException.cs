using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Exceptions
{
    public class QueueingException : Exception
    {
        public QueueingException(string message, Exception ex) : base(message, ex)
        {
        }

        public QueueingException(string message) : base(message)
        {
        }

    }
}
