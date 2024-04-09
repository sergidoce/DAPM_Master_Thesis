using System.ComponentModel.DataAnnotations;

namespace DAPM.ResourceRegistryMS.Api.Models
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Repository Repository { get; set; }
        public ResourceType Type { get; set; }
    }
}
