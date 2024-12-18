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
using DAPM.Authenticator.Data;
using DAPM.Authenticator.Interfaces;

namespace DAPM.Authenticator.Consumers
{
    public class DeleteUserMessageConsumer : IQueueConsumer<DeleteUserMessage>
    {
        private readonly IUserManagerWrapper _usermanager;
        private readonly IQueueProducer<DeleteUserResultMessage> _deleteUserResultProducer;

        public DeleteUserMessageConsumer(IUserManagerWrapper usermanager, IQueueProducer<DeleteUserResultMessage> deleteUserResultProducer)
        {
            _usermanager = usermanager;
            _deleteUserResultProducer = deleteUserResultProducer;
        }

        public async Task ConsumeAsync(DeleteUserMessage message)
        {
            var deleteUserResultMessage = new DeleteUserResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Succeeded = false,
                Message = "This user does not exist in our system, therefore cannot be deleted"
            };
            
            User retreival = await _usermanager.FindByNameAsync(message.UserName);
            if (retreival == null)
            {
                _deleteUserResultProducer.PublishMessage(deleteUserResultMessage);
                return;
            }

            var result = await _usermanager.DeleteAsync(retreival);

            if (result.Succeeded)
            {
                deleteUserResultMessage.Succeeded = true;
                deleteUserResultMessage.Message = "User succesfully deleted";
                _deleteUserResultProducer.PublishMessage(deleteUserResultMessage);
            }
            else
            {
                deleteUserResultMessage.Message = "An unexpected error occurred.";
                _deleteUserResultProducer.PublishMessage(deleteUserResultMessage);
            }
        }
    }
}
