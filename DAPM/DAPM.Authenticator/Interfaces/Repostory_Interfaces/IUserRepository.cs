using DAPM.Authenticator.Models;

namespace DAPM.Authenticator.Interfaces.Repostory_Interfaces
{
    public interface IUserRepository
    {


        public List<User> Users();

        public bool UserExists(string username);

        public User GetUserByName(string username);
        public User GetUserById(int id);

        public int SaveChanges(User user);
    }
}