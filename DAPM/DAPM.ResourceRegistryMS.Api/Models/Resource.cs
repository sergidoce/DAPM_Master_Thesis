
using System.ComponentModel.DataAnnotations;

namespace DAPM.ResourceRegistryMS.Api.Models
{
    public class Resource
    {
        [Key]
        public Repository Repository { get; set; }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ResourceType Type { get; set; }
    }
}
