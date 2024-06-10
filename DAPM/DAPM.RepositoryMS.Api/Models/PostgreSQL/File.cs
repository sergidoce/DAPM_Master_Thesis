using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAPM.RepositoryMS.Api.Models.PostgreSQL
{
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string MongoDbFileId { get; set; }
        [Required]
        public string Extension { get; set; }
    }
}
