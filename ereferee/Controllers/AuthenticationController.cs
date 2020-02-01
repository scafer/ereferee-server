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
        AuthService authService = new AuthService();

        [HttpPost]
        [AllowAnonymous]
        [Route("signIn")]
        public ActionResult<AuthData> SignIn(User userData)
        {
            User user;
            using (var service = new AuthService())
            {
                user = service.SignIn(userData);
            }

            if (user != null)
            {
                var token = authService.GetAuthData(user.id);
                return token;
            }
            return new NotFoundResult();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signUp")]
        public ActionResult<string> SignUp(User user)
        {
            var userService = new UserService();
            var authService = new AuthService();

            var checkEmail = userService.CheckEmailExist(user.email);
            if (checkEmail) return "Email already in use.";

            var checkUsername = userService.CheckUsernameExist(user.username);
            if (checkUsername) return "Username already in use.";

            bool userAdded = authService.SignUp(user);
            if (!userAdded) return "An error occurred while adding the user.";

            return "User added successfully";
        }
    }
}