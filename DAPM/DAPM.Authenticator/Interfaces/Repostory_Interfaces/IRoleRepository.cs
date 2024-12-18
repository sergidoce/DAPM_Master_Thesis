using DAPM.Authenticator.Data;
using DAPM.Authenticator.Models;

namespace DAPM.Authenticator.Interfaces.Repostory_Interfaces
{
    public interface IRoleRepository
    {
        public Role GetRoleById(int id);
        public Role GetRoleByName(string username);
        public int SaveChanges(Role role);

        public bool RoleExists(string username);

        public List<Role> Roles();
    }
}