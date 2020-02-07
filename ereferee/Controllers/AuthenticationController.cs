using ereferee.Models;
using ereferee.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ereferee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                var token = TokenService.GenerateToken(user);
                return token;
            }

            return new NotFoundResult();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signUp")]
        public ActionResult<SvcResult> SignUp(User user)
        {
            using var userService = new UserService();
            using var authService = new AuthService();

            if (userService.CheckEmailExist(user.email)) return SvcResult.Get(1, "Email in use");
            if (userService.CheckUsernameExist(user.username)) return SvcResult.Get(1, "User in use");
            return !authService.SignUp(user) ? SvcResult.Get(1, "Error while adding user") : SvcResult.Get(0, "Success");
        }

        [HttpGet]
        [Authorize]
        [Route("authenticated")]
        public ActionResult<SvcResult> Authenticated()
        {
            return SvcResult.Get(0, "Success");
        }
    }
}