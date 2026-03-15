using WebAPI.Services.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginOutput?> LoginAsync(UserLoginInput userLoginDTO);

        Task SignupAsync(UserSignupInput userSignupDTO);
    }
}
