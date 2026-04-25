using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService, IUserService userService,  ILogger<AuthController> logger) : ControllerBase
    {
        private const string RefreshTokenCookieKey = "refreshToken";


        [HttpPost("login")]
        public async Task<ActionResult<UserLoginPartialOutput>> LoginAsync(UserLoginInput userLoginInput)
        {
            logger.LogInformation("Login requested for user {UserEmail}.", userLoginInput.Email);

            var userLoginOutput = await authService.LoginAsync(userLoginInput);

            if (userLoginOutput == null)
            {
                logger.LogWarning("Login for user {UserEmail} failed, unauthorized.", userLoginInput.Email);

                return Unauthorized();
            }

            this.StoreRefreshTokenCookie(userLoginOutput.RefreshToken);

            return Ok(userLoginOutput.UserLoginPartialOutput);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<RefreshTokenOutput>> RefreshTokenAsync()
        {
            if (!Request.Cookies.TryGetValue(RefreshTokenCookieKey, out var refreshToken))
            {
                logger.LogWarning("No refresh token provided, unauthorized.");

                return Unauthorized("No refresh token provided.");
            }

            var userId = authService.ValidateRefreshToken(refreshToken);

            if (userId == null)
            {
                logger.LogWarning("Invalid refresh token, unauthorized.");

                return Unauthorized("Invalid refresh token.");
            }

            logger.LogInformation("Refreshing token for user {UserId}.", userId.Value);

            var user = await userService.GetUserAsync(userId.Value);

            if (user == null)
            {
                logger.LogWarning("User cannot be found {userId}.", userId.Value);

                return Unauthorized("Invalid user.");
            }

            var newAccessToken = await authService.GenerateAccessTokenAsync(user);

            var newRefreshToken = await authService.GenerateRefreshTokenAsync(user);

            // Rotate the refresh token
            this.StoreRefreshTokenCookie(newRefreshToken);

            return Ok(new RefreshTokenOutput { AccessToken = newAccessToken });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignupAsync(UserSignupInput userSignupInput)
        {
            logger.LogInformation("Signup requested for user {UserEmail}.", userSignupInput.Email);

            var isSuccessful = await authService.SignupAsync(userSignupInput);

            if (!isSuccessful)
            {
                return BadRequest();
            }

            return NoContent();
        }

        private void StoreRefreshTokenCookie(string refreshToken)
        {
            Response.Cookies.Append(RefreshTokenCookieKey, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Only HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF
                Expires = DateTimeOffset.UtcNow.AddHours(2) // expires in 2 hours
            });
        }
    }
}
