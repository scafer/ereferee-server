using ereferee.Models;
using ereferee.Services;

namespace ereferee.Extensions
{
    using System.Security.Claims;

    namespace API.Extensions
    {
        public static class UserExtensions
        {
            public static User GetUser(this ClaimsPrincipal claims)
            {
                using (var agent = new UserService())
                {
                    var claimsIdentity = claims.Identity as ClaimsIdentity;
                    var userId = claimsIdentity.FindFirst("nameid")?.Value;

                    var user = agent.GetUserById(int.Parse(userId));

                    if (user != null)
                    {
                        return user;
                    }

                    return null;
                }
            }
        }
    }
}