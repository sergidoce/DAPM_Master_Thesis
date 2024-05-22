
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAPM.ResourceRegistryMS.Api.Models
{
    public class Repository
    {
        // Attributes (Columns)
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name {  get; set; }
        public int PeerId { get; set; }

        // Navigation Attributes (Foreign Keys)
        [ForeignKey("PeerId")]
        public virtual Peer Peer { get; set; }
    }
}
