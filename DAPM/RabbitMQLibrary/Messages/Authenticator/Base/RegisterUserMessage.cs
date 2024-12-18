using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;

namespace RabbitMQLibrary.Messages.Authenticator.Base
{
    public class RegisterUserMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public string FullName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public string OrganizationName { get; set; } = "";
        public Guid OrganizationId { get; set; }

        public List<string> Roles { get; set; }
    }
}
