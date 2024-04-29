
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAPM.ResourceRegistryMS.Api.Models
{
    public class Resource
    {
        // Attributes (Columns)
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int PeerId { get; set; }
        public int RepositoryId { get; set; }
        public int ResourceTypeId { get; set; }


        // Navigation Attributes (Foreign Keys)

        [ForeignKey("PeerId")]
        public virtual Peer Peer { get; set; }
        [ForeignKey("PeerId, RepositoryId")]
        public virtual Repository Repository { get; set; }
        [ForeignKey("ResourceTypeId")]
        public virtual ResourceType ResourceType { get; set; }
    }
}
