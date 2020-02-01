using ereferee.Helpers;
using ereferee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace ereferee.Services
{
    public class AuthService : BaseService
    {
        JwtSettings settings;
        string jwtSecret;
        int jwtLifeSpan;

        public AuthService()
        {
            settings = GetConfigurations.GetAppSettings().Build().GetSection("JwtSettings").Get<JwtSettings>();
            this.jwtLifeSpan = settings.ValidForMinutes;
            this.jwtSecret = settings.SigningKey;
        }

        public User SignIn(User user)
        {
            using (var agent = new UserService())
            {
                var account = agent.GetUser(user.username);

                if (account != null)
                {
                    var isPasswordEquals = VerifyPassword(user.password, account.password);

                    if (isPasswordEquals)
                    {
                        return account;
                    }
                }
                return null;
            }
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        internal bool SignUp(User user)
        {
            try
            {
                user.password = HashPassword(user.password);
                UserService userService = new UserService();
                userService.AddUser(user);

                NotificationHelper.SendEmail(user.email, "Welcome to eScout", "Welcome to eScout " + user.username);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public ActionResult<AuthData> GetAuthData(int userId)
        {
            var expirationTime = DateTime.UtcNow.AddMinutes(jwtLifeSpan);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = settings.Issuer,
                Audience = settings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new AuthData
            {
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds(),
                Id = userId.ToString()
            };
        }
    }
}