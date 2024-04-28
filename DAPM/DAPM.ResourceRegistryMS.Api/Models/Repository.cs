
using System.ComponentModel.DataAnnotations;

namespace DAPM.ResourceRegistryMS.Api.Models
{
    public class Repository
    {
        [Key]
        public Peer Peer { get; set; }

        [Key]
        public int Id { get; set; }

        public string Name {  get; set; }

        
    }
}
