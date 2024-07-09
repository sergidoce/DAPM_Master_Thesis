using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class StepStatusDTO
    {
        public Guid StepId { get; set; }
        public Guid ExecutionerPeer { get; set; }
        public string StepType { get; set; }
        public TimeSpan ExecutionTime { get; set; }
    }
}
