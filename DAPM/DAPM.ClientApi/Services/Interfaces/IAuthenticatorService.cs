using UtilLibrary;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IAuthenticatorService
    {
        public void SignUp();
        public Guid Login(LoginDto loginDto);
        public Guid RegisterUser(RegistrationDto registerDto);
        public Guid DeleteUser(string userName);
        public Guid AddRoles(List<string> roles);
        public Guid EditAsAdmin(UserEditDto userEditDto);
        public Guid EditAsUser(UserEditDto userEditDto);
        public Guid GetRoles();
        public Guid GetUsers();
        public Guid SetOrganization(Guid organizationId, string organizationName, string userName);
        public Guid SetRoles(string userName, List<string> roles);
    }
}
