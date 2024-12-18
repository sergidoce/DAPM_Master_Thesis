using DAPM.Authenticator.Interfaces;
using DAPM.Authenticator.Models;
using Google.Protobuf;
using Microsoft.AspNetCore.Identity;

namespace DAPM.Authenticator.Data
{
    public class UserManagerWrapper : IUserManagerWrapper
    {
        UserManager<User> _userManager;

        public UserManagerWrapper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public Task<IdentityResult> AddPasswordAsync(User user, string newpassword)
        {
            return _userManager.AddPasswordAsync(user, newpassword);
        }

        public Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return _userManager.AddToRoleAsync(user, role);
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<IdentityResult> DeleteAsync(User user)
        {
            return _userManager.DeleteAsync(user);
        }

        public Task<User> FindByIdAsync(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<User> FindByNameAsync(string name)
        {
            return _userManager.FindByNameAsync(name);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return _userManager.GetRolesAsync(user);
        }

        public Task<IdentityResult> RemoveFromRoleAsync(User user, string removerole)
        {
            return _userManager.RemoveFromRoleAsync(user, removerole);
        }

        public Task<IdentityResult> RemovePasswordAsync(User user)
        {
            return _userManager.RemovePasswordAsync(user);
        }
    }
}
