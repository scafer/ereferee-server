using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    public class TokenConfiguration
    {
        public string Audience { get; }
        public string Issuer { get; }
        public int ValidForMinutes { get; }
        public int RefreshTokenValidForMinutes { get; }
        public SigningCredentials SigningCredentials { get; }
        public string Key { get; set; }

        public DateTime IssuedAt => DateTime.UtcNow;
        public DateTime NotBefore => IssuedAt;
        public DateTime AccessTokenExpiration => IssuedAt.AddMinutes(ValidForMinutes);
        public DateTime RefreshTokenExpiration => IssuedAt.AddMinutes(RefreshTokenValidForMinutes);

        private readonly JwtSettings _appSettings;

        public TokenConfiguration(IConfiguration configuration)
        {
            var setting = configuration.GetSection("JwtSettings");
            _appSettings = setting.Get<JwtSettings>();

            Issuer = _appSettings.Issuer;
            Audience = _appSettings.Audience;
            ValidForMinutes = Convert.ToInt32(_appSettings.ValidForMinutes);
            RefreshTokenValidForMinutes = Convert.ToInt32(_appSettings.RefreshTokenValidForMinutes);
            Key = _appSettings.SigningKey;

            var signingKey = _appSettings.SigningKey;
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
