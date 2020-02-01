using ereferee.Extensions.API.Extensions;
using ereferee.Models;
using ereferee.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ereferee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        AuthService authService = new AuthService();

        [HttpPost]
        [Route("resetPassword")]
        public ActionResult<string> ResetPassword(string username, string email)
        {
            using (var service = new UserService())
            {
                return service.ResetPassword(username, email) ? "true" : "false";
            }
        }

        [HttpPost]
        [Authorize]
        [Route("changePassword")]
        public ActionResult<string> ChangePassword(string oldPassword, string newPassword)
        {
            using (var service = new UserService())
            {
                return service.ChangePassword(oldPassword, newPassword) ? "true" : "false";
            }
        }

        [HttpGet]
        [Authorize]
        [Route("getAllUsers")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var service = new UserService())
                {
                    return service.GetUsers();
                }
            }

            return null;
        }
    }
}