using System.ComponentModel.DataAnnotations;

namespace DAPM.ResourceRegistryMS.Api.Models
{
    public class Peer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApiUrl { get; set; }
    }
}
