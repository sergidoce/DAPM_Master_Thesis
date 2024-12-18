using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;
using UtilLibrary;
using AutoMapper;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using DAPM.Authenticator.Interfaces;

namespace DAPM.Authenticator.Consumers
{
    public class GetUsersMessageConsumer : IQueueConsumer<GetUsersMessage>
    {
        private readonly IUserManagerWrapper _usermanager;
        private readonly IUserRepository _userrepository;
        private readonly IMapper _mapper;
        private readonly IQueueProducer<GetUsersResultMessage> _getUsersResultMessageProducer;

        public GetUsersMessageConsumer(
            IUserManagerWrapper userManager, 
            IUserRepository userRepository, 
            IMapper mapper, 
            IQueueProducer<GetUsersResultMessage> getUsersResultMessageProducer)
        {
            _usermanager = userManager;
            _userrepository = userRepository;
            _mapper = mapper;
            _getUsersResultMessageProducer = getUsersResultMessageProducer;
        }

        public Task ConsumeAsync(GetUsersMessage message)
        {
            List<User> users = _userrepository.Users();

            List<UserDto> usersdto = users.Select(x => {
                UserDto userdtoresp = _mapper.Map<UserDto>(x);
                userdtoresp.Roles = [.. _usermanager.GetRolesAsync(x).GetAwaiter().GetResult()];
                return userdtoresp;
            }).ToList();

            var getUsersResultMessage = new GetUsersResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Succeeded = true,
                Message = JsonConvert.SerializeObject(usersdto)
            };

            _getUsersResultMessageProducer.PublishMessage(getUsersResultMessage);

            return Task.CompletedTask;
        }
    }
}
