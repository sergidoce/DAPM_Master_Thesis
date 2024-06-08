using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class ResourceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public string Type { get; set; }
    }
}
