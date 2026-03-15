using WebAPI.Services.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginOutput?> LoginAsync(UserLoginInput userLoginDTO);

        Task<string?> ValidateRefreshToken(string refreshToken);

        Task<string> GenerateAccessTokenAsync(int userID);

        Task<string> GenerateRefreshTokenAsync(int userID);

        Task SignupAsync(UserSignupInput userSignupDTO);
    }
}
