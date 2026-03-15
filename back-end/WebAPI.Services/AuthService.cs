using WebAPI.Data;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly SandboxContext devSandboxContext;

        public AuthService()
        {
            this.devSandboxContext = new SandboxContext();
        }

        public Task LoginAsync(UserLoginDTO userLoginDTO)
        {
            Console.WriteLine($"Email: {userLoginDTO.Email}");
            Console.WriteLine($"Password: {userLoginDTO.Password}");

            return Task.CompletedTask;
        }

        public Task SignupAsync(UserSignupDTO userSignupDTO)
        {
            return Task.CompletedTask;
            //return devSandboxContext.Users.AddAsync(...);
        }
    }
}
