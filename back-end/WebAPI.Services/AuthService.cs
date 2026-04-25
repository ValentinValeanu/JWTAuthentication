using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public async Task<UserLoginOutput?> LoginAsync(UserLoginInput userLoginDTO)
        {
            var user = await sandboxContext.Users.AsNoTracking()
                                                 .Include(user => user.Roles)
                                                 .FirstOrDefaultAsync(u => u.Email == userLoginDTO.Email);

            if (user == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<UserLoginInput>();

            var result = passwordHasher.VerifyHashedPassword(userLoginDTO, user.Password, userLoginDTO.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return null;
            }

            var accessToken = await GenerateAccessTokenAsync(user);

            var refreshToken = await GenerateRefreshTokenAsync(user);

            var userData = new UserDTO(user.FirstName, user.LastName, user.Email);

            return new UserLoginOutput(
                UserLoginPartialOutput: new UserLoginPartialOutput(userData, accessToken),
                RefreshToken: refreshToken);
        }

        public Task<string> GenerateAccessTokenAsync(User user) =>
            GenerateTokenAsync(user, JwtToken.AccessToken, DateTime.Now.AddMinutes(15));

        public Task<string> GenerateRefreshTokenAsync(User user) =>
            GenerateTokenAsync(user, JwtToken.RefreshToken, DateTime.Now.AddHours(3));

        public Task<string> GenerateTokenAsync(User user, JwtToken jwtToken, DateTime expiration) => Task.Run(() =>
        {
            var claims = new[]
            {
                new Claim("type", jwtToken.ToString()),
                new Claim(ClaimTypes.Role, string.Join(",", user.Roles.Select(role => role.Name))),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        });

        public int? ValidateRefreshToken(string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = JwtSettings.GetTokenValidationParameters();

            try
            {
                var principal = handler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken)
                {
                    return null;
                }

                var isCorrectAlgorithm =
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                var isRefreshToken =
                    jwtToken.Claims.FirstOrDefault(c => c.Type == "type")?.Value == JwtToken.RefreshToken.ToString();

                if (!isCorrectAlgorithm || !isRefreshToken)
                {
                    return null;
                }

                // Token is valid
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                if (!int.TryParse(userId, out var intUserID))
                {
                    return null;
                }

                return intUserID;
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
        }

        public async Task<bool> SignupAsync(UserSignupInput userSignupDTO)
        {
            var passwordHasher = new PasswordHasher<UserSignupInput>();

            var studentRole = 
                await sandboxContext.Roles.AsNoTracking()
                                          .FirstOrDefaultAsync(role => role.Name == UserRole.Student.ToString());

            if (studentRole == null)
            {
                return false;
            }

            await sandboxContext.Users.AddAsync(new User
            {
                Email = userSignupDTO.Email,
                LastName = userSignupDTO.LastName,
                FirstName = userSignupDTO.FirstName,
                Password = passwordHasher.HashPassword(userSignupDTO, userSignupDTO.Password),
                BirthDate = Convert.ToDateTime(userSignupDTO.BirthDate),
                Roles = [studentRole]
            });

            await sandboxContext.SaveChangesAsync();

            return true;
        }
    }
}
