using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SigningKey { get; set; }
        public int ValidForMinutes { get; set; }
        public int RefreshTokenValidForMinutes { get; set; }

    }

    public class AzureSignalR
    {
        public string ConnectionString { get; set; }
    }
}
