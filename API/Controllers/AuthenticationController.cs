using API.DataAgents;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/Auth/")]
    public class AuthenticationController
    {
        AuthService authService = new AuthService();

        [HttpPost]
        [Route("signIn")]
        [AllowAnonymous]
        public ActionResult<AuthData> SignIn(User login)
        {
            var user = new User();

            using (var agent = new AuthenticationAgent())
            {
                user = agent.SignInUser(login);
            }
            
            if (user != null)
            {
                var tokenString = authService.GetAuthData(user.Username);
                return tokenString;
            }

            return new NotFoundResult();
        }

        [HttpPost]
        [Route("signUp")]
        [AllowAnonymous]
        public ActionResult<string> SignUp(User user)
        {
            using (var agent = new AuthenticationAgent())
            {
                var checkEmail = agent.CheckEmailExist(user.Email);
                if (checkEmail) return "Email already in use.";

                var checkUsername = agent.CheckUsernameExist(user.Username);
                if (checkUsername) return "Username already in use.";

                bool userAdded = agent.SignUpUser(user);

                if (!userAdded)
                {
                    return "An error occurred while adding the user.";
                }

                return "User added succefuly";
            }
        }

        [HttpPost]
        [Route("resetPassword")]
        [AllowAnonymous]
        public ActionResult<string> ResetPassword(string username, string email)
        {
            using (var agent = new AuthenticationAgent())
            {
                var checkCredencials = agent.CheckCredencials(username, email);

                if (!checkCredencials) return "Username or Email invalid.";

                string passwordReset = agent.ResetPassword(username, email);

                var sendEmail = agent.SendEmailToUser(username, email, passwordReset);

                return "User added succefuly";
            }
        }

        [HttpPost]
        [Route("changePassword")]
        [Authorize]
        public ActionResult<string> ChangePassword(string username, string oldPassword, string newPassword)
        {
            using (var agent = new AuthenticationAgent())
            {
                var user = agent.GetUser(username);

                if (user == null) return "Username is invalid.";

                bool samePassword = authService.VerifyPassword(oldPassword, user.Password);

                if (!samePassword) return "The password invalid.";

                var sendEmail = agent.ChangePassword(username, user.Email, newPassword);

                return "Password changed succefuly";
            }
        }
    }
}
