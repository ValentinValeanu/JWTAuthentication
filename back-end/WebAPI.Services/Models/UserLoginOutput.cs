namespace WebAPI.Services.Models
{

    public record UserDTO(string FirstName, string LastName, string Email);

    public record UserLoginPartialOutput(UserDTO User, string AccessToken);

    public record UserLoginOutput(UserLoginPartialOutput UserLoginPartialOutput, string RefreshToken);
}
