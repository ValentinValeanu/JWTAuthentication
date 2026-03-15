using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Services.Properties
{
    public static class JwtSettings
    {
        public const string SecretKey = "SuperSecretKey12345SuperSecretKey12345";

        public const string Issuer = "dev_sandbox.com";

        public const string Audience = "dev_sandbox.com";

        public static TokenValidationParameters GetTokenValidationParameters() => new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
        };
    }
}
