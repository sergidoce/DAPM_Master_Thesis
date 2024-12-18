using DAPM.Authenticator.Data;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Models;

namespace DAPM.Authenticator.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private DataContext _datacontext;
        public RoleRepository(DataContext dataContext)
        {
            _datacontext = dataContext;
        }

        public Role GetRoleById(int id)
        {
            return _datacontext.Roles.FirstOrDefault(r => r.Id == id);
        }

        public Role GetRoleByName(string rolename)
        {
            return _datacontext.Roles.FirstOrDefault(r => r.Name == rolename);
        }

        public bool RoleExists(string rolename)
        {
            return GetRoleByName(rolename) != null;
        }

        public List<Role> Roles()
        {
            return _datacontext.Roles.ToList();
        }

        public int SaveChanges(Role role)
        {
            _datacontext.Update(role);
            return _datacontext.SaveChanges();
        }
    }
}
