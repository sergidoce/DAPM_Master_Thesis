using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Implementation
{
    public class QueueingConfigurationSettings
    {
        public string RabbitMqHostname { get; set; }
        public string RabbitMqUsername { get; set; }
        public string RabbitMqPassword { get; set; }
        public int? RabbitMqPort { get; set; }
        public int? RabbitMqConsumerConcurrency { get; set; }
    }

}
