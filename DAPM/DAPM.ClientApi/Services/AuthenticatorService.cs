using DAPM.ClientApi.Services.Interfaces;

namespace DAPM.ClientApi.Services
{
    public class AuthenticatorService : IAuthenticatorService
    {
        private readonly ILogger<AuthenticatorService> _logger;

        AuthenticatorService(ILogger<AuthenticatorService> logger) { _logger = logger; }


        public void AddUser()
        {
        }

        public void LogIn()
        {
 
        }

        public void RemoveUser()
        {
            
        }

        public void SignUp()
        {
        
        }
    }
}
