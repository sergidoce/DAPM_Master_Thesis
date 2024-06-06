using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAPM.RepositoryMS.Api.Models.PostgreSQL
{
    public class Pipeline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid RepositoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PipelineJson { get; set; }

        // Navigation Attributes (Foreign Keys)
        [ForeignKey("RepositoryId")]
        public virtual Repository Repository { get; set; }

    }
}
