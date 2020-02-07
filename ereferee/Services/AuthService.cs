using ereferee.Helpers;
using ereferee.Models;
using System;

namespace ereferee.Services
{
    public class AuthService : BaseService
    {
        public User SignIn(User user)
        {
            using (var agent = new UserService())
            {
                var account = agent.GetUser(user.username);

                if (account != null)
                {
                    var isPasswordEquals = VerifyPassword(user.password, account.password);

                    if (isPasswordEquals)
                    {
                        return account;
                    }
                }
                return null;
            }
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        internal bool SignUp(User user)
        {
            try
            {
                user.password = HashPassword(user.password);
                UserService userService = new UserService();
                userService.CreateUser(user);
                NotificationHelper.SendEmail(user.email, "Welcome to eScout", "Welcome to eScout " + user.username);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}