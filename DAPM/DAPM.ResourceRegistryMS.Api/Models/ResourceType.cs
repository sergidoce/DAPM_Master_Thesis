using System.ComponentModel.DataAnnotations;

namespace DAPM.ResourceRegistryMS.Api.Models
{
    public class ResourceType
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileExtension { get; set; }
    }
}
