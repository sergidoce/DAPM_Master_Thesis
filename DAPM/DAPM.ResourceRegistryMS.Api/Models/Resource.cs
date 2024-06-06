
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAPM.ResourceRegistryMS.Api.Models
{
    public class Resource
    {
        // Attributes (Columns)
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public Guid PeerId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid ResourceTypeId { get; set; }


        // Navigation Attributes (Foreign Keys)

        [ForeignKey("PeerId")]
        public virtual Peer Peer { get; set; }
        [ForeignKey("PeerId, RepositoryId")]
        public virtual Repository Repository { get; set; }
        [ForeignKey("ResourceTypeId")]
        public virtual ResourceType ResourceType { get; set; }
    }
}
