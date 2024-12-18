using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using RabbitMQLibrary.Messages.Authenticator.RoleManagement;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using UtilLibrary;

namespace DAPM.ClientApi.Services
{
    public class AuthenticatorService : IAuthenticatorService
    {
        private readonly ILogger<AuthenticatorService> _logger;
        private readonly ITicketService _ticketService;
        private readonly IQueueProducer<RegisterUserMessage> _registerUserMessageProducer;
        private readonly IQueueProducer<LoginMessage> _loginMessageProducer;
        private readonly IQueueProducer<AddRolesMessage> _addRolesMessageProducer;
        private readonly IQueueProducer<GetRolesMessage> _getRolesMessageProducer;
        private readonly IQueueProducer<DeleteUserMessage> _deleteUserMessageProducer;
        private readonly IQueueProducer<EditAsAdminMessage> _editAsAdminMessageProducer;
        private readonly IQueueProducer<EditAsUserMessage> _editAsUserMessageProducer;
        private readonly IQueueProducer<GetUsersMessage> _getUsersMessageProducer;
        private readonly IQueueProducer<SetOrganizationMessage> _setOrganizationMessageProducer;
        private readonly IQueueProducer<SetRolesMessage> _setRolesMessageProducer;

        public AuthenticatorService(
            ILogger<AuthenticatorService> logger,
            ITicketService ticketService,
            IQueueProducer<RegisterUserMessage> registerUserMessageProducer,
            IQueueProducer<LoginMessage> loginMessageProducer,
            IQueueProducer<AddRolesMessage> addRolesMessageProducer,
            IQueueProducer<GetRolesMessage> getRolesMessageProducer,
            IQueueProducer<DeleteUserMessage> deleteUserMessageProducer,
            IQueueProducer<EditAsAdminMessage> editAsAdminMessageProducer,
            IQueueProducer<EditAsUserMessage> editAsUserMessageProducer,
            IQueueProducer<GetUsersMessage> getUsersMessageProducer,
            IQueueProducer<SetOrganizationMessage> setOrganizationMessageProducer,
            IQueueProducer<SetRolesMessage> setRolesMessageProducer
        ) {
            _logger = logger;
            _ticketService = ticketService; 
            _registerUserMessageProducer = registerUserMessageProducer;
            _loginMessageProducer = loginMessageProducer;
            _addRolesMessageProducer = addRolesMessageProducer;
            _getRolesMessageProducer = getRolesMessageProducer;
            _deleteUserMessageProducer = deleteUserMessageProducer;
            _editAsAdminMessageProducer = editAsAdminMessageProducer;
            _editAsUserMessageProducer = editAsUserMessageProducer;
            _getUsersMessageProducer = getUsersMessageProducer;
            _setOrganizationMessageProducer = setOrganizationMessageProducer;
            _setRolesMessageProducer = setRolesMessageProducer;
        }


        public Guid RegisterUser(RegistrationDto registerDto)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new RegisterUserMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                FullName = registerDto.FullName,
                Password = registerDto.Password,
                UserName = registerDto.UserName,
                OrganizationId = registerDto.OrganizationId,
                OrganizationName = registerDto.OrganizationName,
                Roles = registerDto.Roles
            };

            _registerUserMessageProducer.PublishMessage(message);
            _logger.LogDebug("RegisterUserMessage Enqueued");

            return ticketId;
        }

        public Guid Login(LoginDto loginDto)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new LoginMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                Password = loginDto.Password,
                UserName = loginDto.UserName
            };

            _loginMessageProducer.PublishMessage(message);
            _logger.LogDebug("LoginMessage Enqueued");

            return ticketId;
        }
        
        public void SignUp()
        {
        
        }

        public Guid DeleteUser(string userName)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new DeleteUserMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                UserName = userName
            };

            _deleteUserMessageProducer.PublishMessage(message);
            _logger.LogDebug("DeleteUserMessage Enqueued");

            return ticketId;
        }

        public Guid AddRoles(List<string> roles)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new AddRolesMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                Roles = roles
            };

            _addRolesMessageProducer.PublishMessage(message);
            _logger.LogDebug("AddRolesMessage Enqueued");

            return ticketId;
        }

        public Guid EditAsAdmin(UserEditDto userEditDto)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new EditAsAdminMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                Id = userEditDto.Id,
                FullName = userEditDto.FullName,
                UserName = userEditDto.UserName,
                NewPassword = userEditDto.NewPassword,
                Roles = userEditDto.Roles
            };

            _editAsAdminMessageProducer.PublishMessage(message);
            _logger.LogDebug("EditAsAdminMessage Enqueued");

            return ticketId;
        }

        public Guid EditAsUser(UserEditDto userEditDto)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new EditAsUserMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                Id = userEditDto.Id,
                FullName = userEditDto.FullName,
                UserName = userEditDto.UserName,
                NewPassword = userEditDto.NewPassword,
                Roles = userEditDto.Roles
            };

            _editAsUserMessageProducer.PublishMessage(message);
            _logger.LogDebug("EditAsUserMessage Enqueued");

            return ticketId;
        }

        public Guid GetRoles()
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new GetRolesMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId
            };

            _getRolesMessageProducer.PublishMessage(message);
            _logger.LogDebug("GetRolesMessage Enqueued");

            return ticketId;
        }

        public Guid GetUsers()
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new GetUsersMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId
            };

            _getUsersMessageProducer.PublishMessage(message);
            _logger.LogDebug("GetUsersMessage Enqueued");

            return ticketId;
        }

        public Guid SetOrganization(Guid organizationId, string organizationName, string userName)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new SetOrganizationMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                OrganizationId = organizationId,
                OrganizationName = organizationName,
                UserName = userName
            };

            _setOrganizationMessageProducer.PublishMessage(message);
            _logger.LogDebug("SetOrganizationMessage Enqueued");

            return ticketId;
        }

        public Guid SetRoles(string userName, List<string> roles)
        {
            Guid ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);

            var message = new SetRolesMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                UserName= userName,
                Roles = roles
            };

            _setRolesMessageProducer.PublishMessage(message);
            _logger.LogDebug("SetRolesMessage Enqueued");

            return ticketId;
        }
    }
}
