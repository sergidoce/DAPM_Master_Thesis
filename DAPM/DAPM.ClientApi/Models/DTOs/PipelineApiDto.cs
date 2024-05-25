using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Models.DTOs
{
    public class PipelineApiDto
    {
        public string Name { get; set; }
        public PipelineDTO Pipeline { get; set; }
    }
}
