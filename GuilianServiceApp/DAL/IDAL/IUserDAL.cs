using GuilianServiceApp.Models;

namespace GuilianServiceApp.DAL.IDAL
{
    public interface IUserDAL
    {
        public User Login(string username, string password);
        public bool UserAlreadyExist(User u);
        public bool SaveUser(User user);
        public bool UpdateCredits(User u);
    }
}
