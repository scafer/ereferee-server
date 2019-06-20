using API.Models;
using API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.DataAgents
{
    public class UserAgent : AgentBase
    {
        AuthService authService = new AuthService();

        public UserAgent()
        {

        }

        public UserAgent(AgentBase agent)
            : base(agent) { }

        public User GetUser(string username)
        {
            return DataContext.ExecuteQuery<User>("User/GetUser", username).FirstOrDefault();
        }

        public User[] GetAllUsers()
        {
            return DataContext.ExecuteQuery<User>("User/GetAllUsers").ToArray();
        }

        public User GetUserById(string userId)
        {
            return DataContext.ExecuteQuery<User>("User/GetUserById", userId).FirstOrDefault();
        }

        public string ResetPassword(string username, string email)
        {
            string password = authService.HashPassword("defaultPassword");

            var update = DataContext.ExecuteCommand("User/ResetPassword", username, password, email) > 0;

            if (!update)
            {
                return null;
            }

            return "defaultPassword";
        }

        public bool ChangePassword(string username, string email, string password)
        {
            string passwordHashed = authService.HashPassword(password);

            var update = DataContext.ExecuteCommand("User/ChangePassword", username, passwordHashed, email) > 0;

            if (!update)
            {
                return false;
            }

            return true;
        }

        public bool CheckEmailExist(string email)
        {
            var check = DataContext.ExecuteQuery<int>("User/CheckEmailExist", email).FirstOrDefault();

            return check > 0 ? true : false;
        }

        public bool CheckUsernameExist(string username)
        {
            var check = DataContext.ExecuteQuery<int>("User/CheckUsernameExist", username).FirstOrDefault();

            return check > 0 ? true : false;
        }


        public bool CheckCredencials(string username, string email)
        {
            var check = DataContext.ExecuteQuery<int>("User/CheckCredencials", username, email).FirstOrDefault();

            return check > 0 ? true : false;
        }

        public bool SendEmailToUser(string username, string email, string password)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress("fabioanselmo94@hotmail.com");
                message.Subject = "Reset Email Futsal App";
                message.Body = $"Hello {username}. Your new password is {password}.";
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.live.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("", "");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }

            return true;
        }
    }
}
