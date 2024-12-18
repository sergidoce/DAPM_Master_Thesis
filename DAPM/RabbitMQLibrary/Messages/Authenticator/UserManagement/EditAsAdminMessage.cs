using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;

namespace RabbitMQLibrary.Messages.Authenticator.UserManagement
{
    public class EditAsAdminMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string NewPassword { get; set; } = "";
        public List<string> Roles { get; set; }
    }
}
