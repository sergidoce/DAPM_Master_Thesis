using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;

namespace RabbitMQLibrary.Messages.Authenticator.UserManagement
{
    public class SetOrganizationMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; } = "No affiliation";
        public string UserName { get; set; }
    }
}
