using DAPM.Authenticator.Interfaces;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;

namespace DAPM.Authenticator.Data
{
    public class RoleManagerWrapper : IRoleManagerWrapper
    {

        RoleManager<Role> _rolemanager;

        public RoleManagerWrapper(RoleManager<Role> roleManager )
        {
            _rolemanager = roleManager;   
        }

        public Task<IdentityResult> CreateAsync(Role role)
        {
            return _rolemanager.CreateAsync(role);
        }

        public Task<Role> FindByNameAsync(string role)
        {
            return _rolemanager.FindByNameAsync(role);
        }

        public Task<bool> RoleExistsAsync(string role)
        {
            return _rolemanager.RoleExistsAsync(role);
        }
    }
}
