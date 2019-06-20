using API.DataAgents;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class UserExtensions
    {
        public static User GetUser(this ClaimsPrincipal claims)
        {
            using (var agent = new UserAgent())
            {
                var claimsIdentity = claims.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst("nameid")?.Value;

                var user = agent.GetUserById(userId);

                if (user != null)
                {
                    return user;
                }

                return null;
            }
        }
    }
}
