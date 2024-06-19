using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAPM.RepositoryMS.Api.Models.PostgreSQL
{
    public class Operator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid RepositoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid SourceCodeFileId { get; set; }
        public Guid DockerfileFileId { get; set; }

        [Required]
        public string Type { get; set; }

        // Navigation Attributes (Foreign Keys)
        [ForeignKey("RepositoryId")]
        public virtual Repository Repository { get; set; }

        [ForeignKey("SourceCodeFileId")]
        public virtual File SourceCodeFile { get; set; }

        [ForeignKey("DockerfileFileId")]
        public virtual File DockerfileFile { get; set; }
    }
}
