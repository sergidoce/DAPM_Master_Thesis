namespace DAPM.ClientApi.Models.DTOs
{
    public class ResourceForm
    {
        public string Name { get; set; }
        public string ResourceType { get; set; }
        public IFormFile ResourceFile { get; set; }
    }
}
