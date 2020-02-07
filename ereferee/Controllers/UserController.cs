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
        [HttpPost]
        [Route("resetPassword")]
        public ActionResult<SvcResult> ResetPassword(string username, string email)
        {
            using var service = new UserService();
            var result = service.ResetPassword(username, email);

            if (result)
                return SvcResult.Get(0, "Success");
            else
                return SvcResult.Get(1, "Error");
        }

        [HttpPost]
        [Authorize]
        [Route("changePassword")]
        public ActionResult<SvcResult> ChangePassword(string oldPassword, string newPassword)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            using var userService = new UserService();
            var result = userService.ChangePassword(oldPassword, newPassword);

            if (result)
                return SvcResult.Get(0, "Success");
            else
                return SvcResult.Get(1, "Error");
        }

        [HttpGet]
        [Route("getUserInfo")]
        [Authorize]
        public ActionResult<User> GetUserInfo()
        {
            return User.GetUser();
        }

        [HttpGet]
        [Authorize]
        [Route("getAllUsers")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            /*
            using var service = new UserService();
            return service.GetUsers();
            */
            return new NotFoundResult();
        }
    }
}