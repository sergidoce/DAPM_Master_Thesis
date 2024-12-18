using DAPM.Authenticator.Data;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Models;

namespace DAPM.Authenticator.Repositories
{
    public class UserRepository : IUserRepository

    {
        private DataContext _datacontext;

        public UserRepository(DataContext dataContext)
        {
            _datacontext = dataContext;
        }

        public User GetUserById(int id)
        {
            return _datacontext.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByName(string username)
        {
            return _datacontext.Users.FirstOrDefault(u => u.UserName == username);
        }

        public int SaveChanges(User user)
        {
            _datacontext.Update(user);
            return _datacontext.SaveChanges();
        }

        public bool UserExists(string username)
        {
            return GetUserByName(username) != null;
        }

        public List<User> Users()
        {
            return _datacontext.Users.ToList();
        }
    }
}
