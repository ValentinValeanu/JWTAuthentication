using System.ComponentModel.DataAnnotations;

namespace WebAPI.Services.Models
{
    public class UserLoginInput()
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
