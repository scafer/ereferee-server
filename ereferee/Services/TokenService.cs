using ereferee.Helpers;
using ereferee.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ereferee.Services
{
    public static class TokenService
    {
        public static AuthData GenerateToken(User user)
        {
            var settings = Configurations.GetAppSettings().Build().GetSection("JwtSettings").Get<JwtSettings>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(settings.SigningKey);
            var expirationTime = DateTime.UtcNow.AddMinutes(settings.ValidForMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString())
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthData
            {
                Token = tokenHandler.WriteToken(token),
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds(),
                Id = user.id.ToString()
            };
        }
    }

    public static class UserExtensions
    {
        public static User GetUser(this ClaimsPrincipal claims)
        {
            using (var agent = new UserService())
            {
                var claimsIdentity = claims.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Name;

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