using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAPM.RepositoryMS.Api.Models.PostgreSQL
{
    public class Resource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid RepositoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid FileId { get; set; }

        [Required]
        public string Type { get; set; }

        // Navigation Attributes (Foreign Keys)
        [ForeignKey("RepositoryId")]
        public virtual Repository Repository { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }

    }
}
