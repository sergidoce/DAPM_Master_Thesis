using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models
{
    public class SendResourceToPeerResultDto
    {
        public Guid StepId { get; set; }
        public bool Succeeded { get; set; }
    }
}
