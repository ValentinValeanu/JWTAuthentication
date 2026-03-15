using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        private readonly ILogger<AuthController> _logger = logger;

        private const string RefreshTokenCookieKey = "refreshToken";


        [HttpPost("login")]
        public async Task<ActionResult<UserLoginPartialOutput>> LoginAsync(UserLoginInput userLoginInput)
        {
            _logger.LogInformation("Login requested for user {UserEmail}.", userLoginInput.Email);

            var userLoginOutput = await _authService.LoginAsync(userLoginInput);

            if (userLoginOutput == null)
            {
                _logger.LogWarning("Login for user {UserEmail} failed, unauthorized.", userLoginInput.Email);

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
                _logger.LogWarning("No refresh token provided, unauthorized.");

                return Unauthorized("No refresh token provided.");
            }

            var tokenUserID = await _authService.ValidateRefreshToken(refreshToken);

            if (tokenUserID == null || !int.TryParse(tokenUserID, out var userID))
            {
                _logger.LogWarning("Invalid refresh token, unauthorized.");

                return Unauthorized("Invalid refresh token.");
            }

            _logger.LogInformation("Refreshing token for user {UserID}.", userID);

            var newAccessToken = await _authService.GenerateAccessTokenAsync(userID);

            var newRefreshToken = await _authService.GenerateRefreshTokenAsync(userID);

            // Rotate the refresh token
            this.StoreRefreshTokenCookie(newRefreshToken);

            return Ok(new RefreshTokenOutput { AccessToken = newAccessToken });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignupAsync(UserSignupInput userSignupInput)
        {
            _logger.LogInformation("Signup requested for user {UserEmail}.", userSignupInput.Email);

            await _authService.SignupAsync(userSignupInput);

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
