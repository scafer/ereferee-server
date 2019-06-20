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
    public class AuthenticationAgent : AgentBase
    {
        public AuthenticationAgent()
        {

        }

        public AuthenticationAgent(AgentBase agent)
          : base(agent) { }

        AuthService authService = new AuthService();

        public User SignInUser(User login)
        {
            using (var agent = new UserAgent())
            {
                var account = agent.GetUser(login.Username);

                if (account != null)
                {
                    var isPasswordEquals = authService.VerifyPassword(login.Password, account.Password);

                    if (isPasswordEquals)
                    {
                        return account;
                    }
                }

                return null;
            }
        }

        public bool SignUpUser(User login)
        {
            Identity addUser = DataContext.ExecuteInsertFromQuery("User/AddUser", login.Username, authService.HashPassword(login.Password), login.Email);

            if (addUser.Id == 0)
            {
                return false;
            }

            return true;

        }
                
    }
}
