using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TweetApp_Models;


namespace TweetApp_Services.IRepository
{
    public interface IUserRepo
    {
        Task<bool> CreateNewUser(User user);
        Task<User> EditUser(User user);
        Task<bool> DeleteUser(string loginId);
        Task<bool> IsUniqueUserByEmail(string emailId);
        Task<bool> IsUniquesUserByLoginId(string loginId);
        Task<bool> Authenticate(string loginId, string password);
        Task<bool> ForgotPassword(string loginId, string password);
        Task<User> ViewUser(string loginId);
        Task<List<User>> ViewAllUsers();
    }
}
