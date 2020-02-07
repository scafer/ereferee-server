using ereferee.Helpers;
using ereferee.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ereferee.Services
{
    public class UserService : BaseService
    {
        DataContext db;

        public UserService()
        {
            db = new DataContext();
        }

        public User CreateUser(User user)
        {
            db.users.Add(user);
            db.SaveChanges();
            return user;
        }

        public User GetUser(string username)
        {
            return db.users.FirstOrDefault(u => u.username == username);
        }

        public List<User> GetUsers()
        {
            return db.users.ToList();
        }

        public User GetUserById(int id)
        {
            return db.users.FirstOrDefault(u => u.id == id);
        }

        public bool ResetPassword(string username, string email)
        {
            var user = db.users.FirstOrDefault(u => u.username == username || u.email == email);

            if (user != null)
            {
                var generatedPassword = PasswordGenerator();
                user.password = new AuthService().HashPassword(generatedPassword);
                db.users.Update(user);
                db.SaveChanges();
                NotificationHelper.SendEmail(user.email, "New eScout Password", generatedPassword);
                return true;
            }

            return false;
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return true;
        }

        public bool CheckEmailExist(string email)
        {
            var check = db.users.FirstOrDefault(u => u.email == email);
            return check != null;
        }

        public bool CheckUsernameExist(string username)
        {
            var check = db.users.FirstOrDefault(u => u.username == username);
            return check != null;
        }

        public bool CheckCredentials(string username, string email)
        {
            var check = db.users.FirstOrDefault(u => u.username == username || u.email == email);
            return check != null;
        }

        private string PasswordGenerator()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}