namespace DAPM.ClientApi.Models.DTOs
{
    public class OperatorForm
    {
        public string Name { get; set; }
        public string ResourceType { get; set; }
        public IFormFile SourceCodeFile { get; set; }
        public IFormFile DockerfileFile { get; set; }
    }
}
