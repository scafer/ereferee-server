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
    [Route("api/auth/")]
    public class AuthenticationController : ControllerBase
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
                var tokenString = authService.GetAuthData(user.UserID);
                return tokenString;
            }

            return new NotFoundResult();
        }

        [HttpPost]
        [Route("signUp")]
        [AllowAnonymous]
        public ActionResult<string> SignUp(User user)
        {
            using (var agent = new UserAgent())
            {
                using (var authAgent = new AuthenticationAgent(agent))
                {

                    var checkEmail = agent.CheckEmailExist(user.Email);
                    if (checkEmail) return "Email already in use.";

                    var checkUsername = agent.CheckUsernameExist(user.Username);
                    if (checkUsername) return "Username already in use.";

                    bool userAdded = authAgent.SignUpUser(user);

                    if (!userAdded)
                    {
                        return "An error occurred while adding the user.";
                    }

                    return "User added succefuly";
                }
            }
        }
    }
}
