using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class ResourceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrganizationId { get; set; }
        public int RepositoryId { get; set; }
        public string Type { get; set; }
        public string Extension { get; set; }
    }
}
