using System.ComponentModel;

namespace WebAPI.Services.Properties
{
    public enum JwtToken
    {
        [Description("access-token")]
        AccessToken,
        [Description("refresh-token")]
        RefreshToken
    }
}
