using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Data;
using WebAPI.Data.Entities;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;
using WebAPI.Services.Properties;

namespace WebAPI.Services
{
    public class AuthService(SandboxContext sandboxContext) : IAuthService
    {
        private readonly SandboxContext _sandboxContext = sandboxContext;

        public async Task<UserLoginOutput?> LoginAsync(UserLoginInput userLoginDTO)
        {
            var user = await _sandboxContext.Users.FirstOrDefaultAsync(u => u.Email == userLoginDTO.Email);

            if (user == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<UserLoginInput>();

            var result = passwordHasher.VerifyHashedPassword(userLoginDTO, user.Password, userLoginDTO.Password);

            if (result == PasswordVerificationResult.Success)
            {
                var userData = new UserDTO(user.FirstName, user.LastName, user.Email);

                var accessToken = await GenerateAccessTokenAsync(user.Id);

                var refreshToken = await GenerateRefreshTokenAsync(user.Id);

                return new UserLoginOutput(
                    UserLoginPartialOutput: new UserLoginPartialOutput(userData, accessToken),
                    RefreshToken: refreshToken);
            }

            return null;
        }

        public Task<string?> ValidateRefreshToken(string refreshToken) => Task.Run(() =>
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = JwtSettings.GetTokenValidationParameters();

            try
            {
                var principal = handler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken &&
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) &&
                    jwtToken.Claims.FirstOrDefault(c => c.Type == "type")?.Value == JwtToken.RefreshToken.ToString())
                {
                    // Token is valid
                    var userID = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                    return userID;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                //Token expired
            }
            catch (SecurityTokenException)
            {
                //Invalid token
            }

            return null;
        });

        public Task<string> GenerateAccessTokenAsync(int userID) => Task.Run(() =>
        {
            var claims = new[]
            {
                new Claim("type", JwtToken.AccessToken.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        });

        public Task<string> GenerateRefreshTokenAsync(int userID) => Task.Run(() =>
        {
            var claims = new[]
            {
                new Claim("type", JwtToken.RefreshToken.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        });

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
