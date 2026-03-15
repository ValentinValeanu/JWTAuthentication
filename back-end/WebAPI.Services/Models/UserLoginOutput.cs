namespace WebAPI.Services.Models
{

    public record UserDTO(string FirstName, string LastName, string Email);

    public record UserLoginOutput(UserDTO User, string AccessToken);
}
