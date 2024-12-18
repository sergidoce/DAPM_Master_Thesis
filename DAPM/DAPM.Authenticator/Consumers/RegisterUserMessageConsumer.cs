using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Services;
using UtilLibrary.Interfaces;
using UtilLibrary.models;
using RabbitMQLibrary.Messages.ClientApi;
using DAPM.Authenticator.Interfaces;

public class RegisterUserMessageConsumer : IQueueConsumer<RegisterUserMessage>
{
    private readonly ILogger<RegisterUserMessageConsumer> _logger;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IUserManagerWrapper _userManager;
    private readonly IRoleManagerWrapper _roleManager;
    private readonly ITokenService _tokenService;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userrepository;
    private readonly IQueueProducer<RegisterUserResultMessage> _registerUserResultMessageProducer;

    public RegisterUserMessageConsumer(
        ILogger<RegisterUserMessageConsumer> logger,
        IMapper mapper,
        IConfiguration configuration,
        IUserManagerWrapper userManager,
        IUserRepository userRepository,
        IRoleManagerWrapper roleManager,
        ITokenService tokenService,
        IIdentityService identityService,
        IQueueProducer<RegisterUserResultMessage> registerUserResultMessageProducer)
    {
        _logger = logger;
        _mapper = mapper;
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _identityService = identityService;
        _userrepository = userRepository;
        _registerUserResultMessageProducer = registerUserResultMessageProducer;
    }

    private async Task<bool> UserExistsAsync(string name)
    {
        return await _userManager.FindByNameAsync(name) != null;
    }

    public async Task ConsumeAsync(RegisterUserMessage message)
    {
        var user = _mapper.Map<User>(message);

        if (!await UserExistsAsync(message.UserName))
        {
            var registerUserResult = await _userManager.CreateAsync(user, message.Password);

            var registerUserResultMessage = new RegisterUserResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Message = "",
                Succeeded = false
            };

            if (!registerUserResult.Succeeded)
            {
                registerUserResultMessage.Message = string.Join("; ", registerUserResult.Errors.Select(e => e.Description));
                _registerUserResultMessageProducer.PublishMessage(registerUserResultMessage);
                return;
            }

            foreach (var role in message.Roles)
            {
                var addRoleResult = await _userManager.AddToRoleAsync(user, role);
                if (!addRoleResult.Succeeded)
                {
                    registerUserResultMessage.Message = string.Join("; ", addRoleResult.Errors.Select(e => e.Description));
                    _registerUserResultMessageProducer.PublishMessage(registerUserResultMessage);
                    return;
                }
            }

            Identity identity = _identityService.GetIdentity();
            user.OrganizationId = (Guid)identity.Id;
            user.OrganizationName = identity.Name;
            _userrepository.SaveChanges(user);

            registerUserResultMessage.Message = "Okily dokily";
            registerUserResultMessage.Succeeded = true;
            _registerUserResultMessageProducer.PublishMessage(registerUserResultMessage);
        }
        else
        {
            var registerUserResultMessage = new RegisterUserResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Message = "Username is already in use",
                Succeeded = false
            };

            _registerUserResultMessageProducer.PublishMessage(registerUserResultMessage);
        }
    }
}

