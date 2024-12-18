using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using DAPM.Authenticator.Interfaces;

namespace DAPM.Authenticator.Consumers
{
    public class SetRolesMessageConsumer : IQueueConsumer<SetRolesMessage>
    {
        private readonly IRoleManagerWrapper _rolemanager;
        private readonly IUserManagerWrapper _usermanager;
        private readonly IUserRepository _userrepository;
        private readonly IQueueProducer<SetRolesResultMessage> _setRolesResultProducer;

        public SetRolesMessageConsumer(
            IUserManagerWrapper usermanager, 
            IRoleManagerWrapper rolemanager, 
            IUserRepository userrepository, 
            IQueueProducer<SetRolesResultMessage> setRolesResultProducer)
        {
            _usermanager = usermanager;
            _userrepository = userrepository;
            _rolemanager = rolemanager;
            _setRolesResultProducer = setRolesResultProducer;
        }

        public async Task ConsumeAsync(SetRolesMessage message)
        {
            var setRolesResultMessage = new SetRolesResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Succeeded = false,
                Message = "This user does not exist in our system, therefore we cannot add roles to them"
            };
            
            User retreival = await _usermanager.FindByNameAsync(message.UserName);
            if (retreival == null)
            {
                _setRolesResultProducer.PublishMessage(setRolesResultMessage);
                return;
            }

            foreach (string role in message.Roles)
            {
                Role result = await _rolemanager.FindByNameAsync(role);
                if (result == null)
                {
                    setRolesResultMessage.Message = "Attempting to add non existent role, all roles in list must exist";
                    _setRolesResultProducer.PublishMessage(setRolesResultMessage);
                    return;
                }
            }

            foreach (var item in message.Roles)
            {
                await _usermanager.AddToRoleAsync(retreival, item);
            }

            setRolesResultMessage.Succeeded = true;
            setRolesResultMessage.Message = "succesfully added specified roles";
            _setRolesResultProducer.PublishMessage(setRolesResultMessage);
        }
    }
}
