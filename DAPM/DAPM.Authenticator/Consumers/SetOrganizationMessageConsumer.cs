using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;
using UtilLibrary;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using DAPM.Authenticator.Interfaces;

namespace DAPM.Authenticator.Consumers
{
    public class SetOrganizationMessageConsumer : IQueueConsumer<SetOrganizationMessage>
    {
        private readonly IUserManagerWrapper _usermanager;
        private readonly IUserRepository _userrepository;
        private readonly IQueueProducer<SetOrganizationResultMessage> _setOrganizationResultProducer;

        public SetOrganizationMessageConsumer(IUserManagerWrapper usermanager, IUserRepository userrepository, IQueueProducer<SetOrganizationResultMessage> setOrganizationResultProducer)
        {
            _usermanager = usermanager;
            _userrepository = userrepository;
            _setOrganizationResultProducer = setOrganizationResultProducer;
        }

        public async Task ConsumeAsync(SetOrganizationMessage message)
        {
            var setOrganizationResultMessage = new SetOrganizationResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Succeeded = false,
                Message = "This user does not exist in our system, therefore we cannot set their org"
            };

            User retreival = await _usermanager.FindByNameAsync(message.UserName);
            if (retreival == null)
            {
                _setOrganizationResultProducer.PublishMessage(setOrganizationResultMessage);
                return;
            }

            retreival.OrganizationId = message.OrganizationId;
            retreival.OrganizationName = message.OrganizationName;

            _userrepository.SaveChanges(retreival);

            setOrganizationResultMessage.Succeeded = true;
            setOrganizationResultMessage.Message = $"successfully assigned {message.UserName} a new organisation";

            _setOrganizationResultProducer.PublishMessage(setOrganizationResultMessage);
        }
    }
}
