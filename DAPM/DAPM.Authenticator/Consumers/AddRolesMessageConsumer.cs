using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Authenticator.RoleManagement;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;
using RabbitMQLibrary.Messages.Authenticator.Base;
using DAPM.Authenticator.Interfaces;

namespace DAPM.Authenticator.Consumers
{
    public class AddRolesMessageConsumer : IQueueConsumer<AddRolesMessage>
    {
        private readonly IQueueProducer<AddRolesResultMessage> _addRolesResultProducer;
        private readonly IRoleManagerWrapper _rolemanager;

        public AddRolesMessageConsumer(IQueueProducer<AddRolesResultMessage> addRolesResultProducer, IRoleManagerWrapper rolemanager)
        {
            _addRolesResultProducer = addRolesResultProducer;
            _rolemanager = rolemanager;
        }

        public async Task ConsumeAsync(AddRolesMessage message)
        {
            try
            {
                foreach (var role in message.Roles)
                {
                    await _rolemanager.CreateAsync(new Role { Name = role });
                }

                var addRolesResultMessage = new AddRolesResultMessage
                {
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Succeeded = true,
                    Message = "Added the roles"
                };

                _addRolesResultProducer.PublishMessage(addRolesResultMessage);
            }
            catch (Exception ex)
            {
                var addRolesResultMessage = new AddRolesResultMessage
                {
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Succeeded = false,
                    Message = ex.Message
                };

                _addRolesResultProducer.PublishMessage(addRolesResultMessage);
            }
        }
    }
}
