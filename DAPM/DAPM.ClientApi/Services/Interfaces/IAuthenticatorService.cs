namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IAuthenticatorService
    {
        public void SignUp();
        public void LogIn();
        public void AddUser();
        public void RemoveUser();
    }
}
