using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Authenticator.RoleManagement;
using DAPM.Authenticator.Models;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;

namespace DAPM.Authenticator.Consumers
{
    public class GetRolesMessageConsumer : IQueueConsumer<GetRolesMessage>
    {
        private readonly IRoleRepository _rolerepository;
        private readonly IQueueProducer<GetRolesResultMessage> _getRolesResultMessageProducer;

        public GetRolesMessageConsumer(IRoleRepository rolerepository, IQueueProducer<GetRolesResultMessage> getRolesResultMessageProducer)
        {
            _rolerepository = rolerepository;
            _getRolesResultMessageProducer = getRolesResultMessageProducer;
        }

        public Task ConsumeAsync(GetRolesMessage message)
        {
            List<Role> roles = _rolerepository.Roles();

            var getRolesResultMessage = new GetRolesResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Succeeded = true,
                Message = JsonConvert.SerializeObject(roles.Select(role => new { RoleName = role.Name }))
            };

            _getRolesResultMessageProducer.PublishMessage(getRolesResultMessage);

            return Task.CompletedTask;
        }
    }
}
