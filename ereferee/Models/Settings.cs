namespace ereferee.Models
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

    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; } = "bearer";
        public long ExpiresIn { get; set; }
    }
}
