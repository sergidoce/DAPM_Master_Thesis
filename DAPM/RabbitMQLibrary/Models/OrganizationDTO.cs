using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class OrganizationDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
    }
}
