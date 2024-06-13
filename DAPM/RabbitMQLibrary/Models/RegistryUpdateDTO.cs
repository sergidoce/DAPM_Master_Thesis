using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class RegistryUpdateDTO
    {
        public IEnumerable<OrganizationDTO> Organizations { get; set; }
        public IEnumerable<RepositoryDTO> Repositories { get; set; }
        public IEnumerable<ResourceDTO> Resources { get; set; }
        public IEnumerable<PipelineDTO> Pipelines { get; set; }
    }
}
