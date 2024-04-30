using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class RepositoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PeerId { get; set; }
    }
}
