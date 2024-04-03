using System.ComponentModel.DataAnnotations;

namespace DAPM.ResourceRegistryMS.Api.Models
{
    public class Resource
    {
        [Key]
        public required string Name { get; set; }
        public int Resource_id { get; set; }
        public string Repository_id { get; set; } = string.Empty;
        public string Repository_url {  get; set; } = string.Empty;
    }
}
