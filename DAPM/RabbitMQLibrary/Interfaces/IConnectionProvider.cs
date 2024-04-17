using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQLibrary.Interfaces
{
    public interface IConnectionProvider
    {
        IConnection GetConnection();
    }
}
