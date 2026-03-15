namespace WebAPI.Services.Models
{
    public record UserSignupDTO(string FirstName, string LastName, string BirthDate, string Email, string Password);
}