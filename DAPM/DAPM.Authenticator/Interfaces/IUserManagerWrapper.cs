using DAPM.Authenticator.Models;
using Google.Protobuf;
using Microsoft.AspNetCore.Identity;

namespace DAPM.Authenticator.Interfaces
{
    public interface IUserManagerWrapper
    {
        Task<User> FindByNameAsync(string name);

        Task<IdentityResult> AddToRoleAsync(User user, string role);

        Task<IdentityResult> CreateAsync(User user, string password);

        Task<IList<string>> GetRolesAsync(User user);

        Task<IdentityResult> DeleteAsync(User user);

        Task<User> FindByIdAsync(string id);

        Task<IdentityResult> RemoveFromRoleAsync(User user, string removerole);

        Task<IdentityResult> RemovePasswordAsync(User user);

        Task<IdentityResult> AddPasswordAsync(User user, string newpassword);


        Task<bool> CheckPasswordAsync(User user, string password);
    }
}