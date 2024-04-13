using DAPM.ClientApi.Services.Interfaces;

namespace DAPM.ClientApi.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

    }
}
