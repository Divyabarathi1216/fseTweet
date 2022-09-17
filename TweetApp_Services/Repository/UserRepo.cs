using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TweetApp_DataAccess;
using TweetApp_Models;
using TweetApp_Services.IRepository;

namespace TweetApp_Services.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext _dbContext;

        public UserRepo(DataContext dataContext)
        {
            _dbContext = dataContext;
        }


        /// <summary>
        /// Crete new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CreateNewUser(User user)
        {
            bool res = false;
            try
            {
                user.password = EncryptPassword(user.password);
                user.userCreatedDate = DateTime.Now;
                _dbContext.users.Add(user);
                await _dbContext.SaveChangesAsync();
                res = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return res;
        }

        /// <summary>
        /// Edit user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> EditUser(User user)
        {

            try
            {
                user.password = EncryptPassword(user.password);
                user.userModifiedDate = DateTime.Now;
                _dbContext.users.Update(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;

        }

        /// <summary>
        /// View user based on Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> ViewUser(string loginId)
        {
            User user = new User();
            try
            {
                user = await _dbContext.users.FirstOrDefaultAsync(x => x.loginId == loginId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;

        }

        /// <summary>
        /// View all users
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> ViewAllUsers()
        {
            List<User> userList = new List<User>();
            try
            {
                userList = await _dbContext.users.ToListAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return userList;
        }


        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> Authenticate(string userId, string password)
        {
            bool res = false;
            try
            {
                string encryptPwd = EncryptPassword(password);
                res = await _dbContext.users.FirstOrDefaultAsync(x => x.loginId == userId && x.password == encryptPwd) != null ? true : false;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }


        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> ForgotPassword(string userId, string password)
        {
            bool res = false;
            try
            {
                var user = await ViewUser(userId);
                if (user != null)
                {
                    user.password =EncryptPassword(password);
                    user.userModifiedDate = DateTime.Now;
                    _dbContext.users.Update(user);
                    await _dbContext.SaveChangesAsync();
                }
                res = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }


        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(string loginId)
        {
            bool res = false;
            try
            {
                User user = new User();
                user = await _dbContext.users.FirstOrDefaultAsync(x => x.loginId == loginId);
                if (user != null)
                {
                    _dbContext.users.Remove(user);
                    _dbContext.SaveChanges();
                    res = true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }


        /// <summary>
        /// Is unique email Id
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public async Task<bool> IsUniqueUserByEmail(string emailId)
        {
            bool res = false;
            try
            {
                res = await _dbContext.users.FirstOrDefaultAsync(x => x.emailId == emailId) != null ? false : true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// Is unique login id
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public async Task<bool> IsUniquesUserByLoginId(string loginId)
        {
            bool res = false;
            try
            {
                res = await _dbContext.users.FirstOrDefaultAsync(x => x.loginId == loginId) != null ? false : true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }


        private string EncryptPassword(string password)
        {
            string mes = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            mes = Convert.ToBase64String(encode);
            return mes;
        }

        
    }
}
