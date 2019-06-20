using API.DataAgents;
using API.Extensions;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/user/")]
    public class UserController: ControllerBase
    {
        AuthService authService = new AuthService();

        [HttpPost]
        [Route("resetPassword")]
        [AllowAnonymous]
        public ActionResult<string> ResetPassword(string username, string email)
        {
            using (var agent = new UserAgent())
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
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            using (var agent = new UserAgent())
            {
                var userInfo = agent.GetUser(username);

                if (userInfo == null) return "Username is invalid.";

                bool samePassword = authService.VerifyPassword(oldPassword, userInfo.Password);

                if (!samePassword) return "The password invalid.";

                var sendEmail = agent.ChangePassword(username, userInfo.Email, newPassword);

                return "Password changed succefuly";
            }
        }

        [HttpGet]
        [Route("getUserInfo")]
        [Authorize]
        public ActionResult<User> GetUserInfo()
        {
            var user = User.GetUser();

            return user;
        }

        [HttpGet]
        [Route("getAllUsers")]
        [Authorize]
        public ActionResult<User[]> GetAllUsers()
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var agent = new UserAgent())
                {
                    return agent.GetAllUsers();
                }
            }

            return null;
        }
    }
}
