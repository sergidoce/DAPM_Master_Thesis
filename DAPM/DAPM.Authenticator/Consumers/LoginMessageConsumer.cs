using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Authenticator.Base;
using DAPM.Authenticator.Models;
using DAPM.Authenticator.Services;
using Microsoft.AspNetCore.Identity;
using UtilLibrary;
using AutoMapper;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using DAPM.Authenticator.Interfaces;

namespace DAPM.Authenticator.Consumers
{
    public class LoginMessageConsumer : IQueueConsumer<LoginMessage>
    {
        private readonly ILogger<LoginMessageConsumer> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserManagerWrapper _userManager;
        private readonly IRoleManagerWrapper _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userrepository;
        private readonly IQueueProducer<LoginResultMessage> _loginResultMessageProducer;

        public LoginMessageConsumer(
            ILogger<LoginMessageConsumer> logger,
            IMapper mapper,
            IConfiguration configuration,
            IUserManagerWrapper userManager,
            IUserRepository userRepository,
            IRoleManagerWrapper roleManager,
            ITokenService tokenService,
            IQueueProducer<LoginResultMessage> loginResultMessage)
        {
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _userrepository = userRepository;
            _loginResultMessageProducer = loginResultMessage;
        }

        public async Task ConsumeAsync (LoginMessage message)
        {
            User retreival = await _userManager.FindByNameAsync(message.UserName);

            var loginResultMessage = new LoginResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Succeeded = false,
                Message = ""
            };

            if (retreival == null)
            {
                loginResultMessage.Message = "This user does not exist in our system";
                _loginResultMessageProducer.PublishMessage(loginResultMessage);
                return;
            }

            bool result = await _userManager.CheckPasswordAsync(retreival, message.Password);

            if (!result)
            {
                loginResultMessage.Message = "Invalid password";
                _loginResultMessageProducer.PublishMessage(loginResultMessage);
            }
            else
            {

                UserDto response = _mapper.Map<UserDto>(retreival);
                List<string> roles = [.. await _userManager.GetRolesAsync(retreival)];
                string token = await _tokenService.CreateToken(retreival);
                response.Roles = roles;
                response.Token = token;

                loginResultMessage.Succeeded = true;
                loginResultMessage.Message = JsonConvert.SerializeObject(response);
                _loginResultMessageProducer.PublishMessage(loginResultMessage);
            }
        }
    }
}
