using ereferee.Models;
using ereferee.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ereferee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("signIn")]
        public ActionResult<AuthData> SignIn(User userData)
        {
            using var service = new AuthService();
            var user = service.SignIn(userData);

            if (user != null)
            {
                var token = service.GetAuthData(user.id);
                return token;
            }

            return new NotFoundResult();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signUp")]
        public ActionResult<Result> SignUp(User user)
        {
            using var userService = new UserService();
            using var authService = new AuthService();

            if (userService.CheckEmailExist(user.email)) return Result.Get(1, "Email in use");
            if (userService.CheckUsernameExist(user.username)) return Result.Get(1, "User in use");
            return !authService.SignUp(user) ? Result.Get(1, "Error while adding user") : Result.Get(0, "User added");
        }
    }
}