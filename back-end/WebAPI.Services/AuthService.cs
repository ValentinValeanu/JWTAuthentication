using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Data.Entities;
using WebAPI.Services.Helpers;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Services
{
    public class AuthService(SandboxContext sandboxContext) : IAuthService
    {
        private readonly SandboxContext _sandboxContext = sandboxContext;

        public async Task<UserLoginOutput?> LoginAsync(UserLoginInput userLoginDTO)
        {
            var passwordHasher = new PasswordHasher<UserLoginInput>();

            var user = await _sandboxContext.Users.FirstOrDefaultAsync(u => u.Email == userLoginDTO.Email);

            if (user == null)
            {
                return null;
            }

            var result = passwordHasher.VerifyHashedPassword(userLoginDTO, user.Password, userLoginDTO.Password);

            if (result == PasswordVerificationResult.Success)
            {
                return new UserLoginOutput(
                    User: new UserDTO(user.FirstName, user.LastName, user.Email), 
                    AccessToken: JwtTokenGenerator.Generate(user.Id));
            }

            return null;
        }

        public async Task SignupAsync(UserSignupInput userSignupDTO)
        {
            var passwordHasher = new PasswordHasher<UserSignupInput>();

            await _sandboxContext.Users.AddAsync(new User
            {
                Email = userSignupDTO.Email,
                LastName = userSignupDTO.LastName,
                FirstName = userSignupDTO.FirstName,
                Password = passwordHasher.HashPassword(userSignupDTO, userSignupDTO.Password),
                BirthDate = Convert.ToDateTime(userSignupDTO.BirthDate)
            });

            await _sandboxContext.SaveChangesAsync();
        }
    }
}
