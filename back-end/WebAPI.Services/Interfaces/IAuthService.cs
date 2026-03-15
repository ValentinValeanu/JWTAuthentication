using WebAPI.Services.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task LoginAsync(UserLoginDTO userLoginDTO);

        Task SignupAsync(UserSignupDTO userSignupDTO);
    }
}
