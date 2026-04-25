using WebAPI.Data.Entities;
using WebAPI.Services.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginOutput?> LoginAsync(UserLoginInput userLoginDTO);

        int? ValidateRefreshToken(string refreshToken);

        Task<string> GenerateAccessTokenAsync(User user);

        Task<string> GenerateRefreshTokenAsync(User user);

        Task<bool> SignupAsync(UserSignupInput userSignupDTO);
    }
}
