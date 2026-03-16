using System.ComponentModel;

namespace WebAPI.Services.Properties
{
    internal enum JwtToken
    {
        [Description("access-token")]
        AccessToken,
        [Description("refresh-token")]
        RefreshToken
    }
}
