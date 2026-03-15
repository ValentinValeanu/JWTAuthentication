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

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginOutput>> LoginAsync(UserLoginInput userLoginInput)
        {
            _logger.LogInformation("Login requested for user {UserEmail}.", userLoginInput.Email);

            var userLoginOutput = await _authService.LoginAsync(userLoginInput);

            if (userLoginOutput == null)
            {
                _logger.LogWarning("Login for user {UserEmail} failed, unauthorized.", userLoginInput.Email);

                return Unauthorized();
            }

            return Ok(userLoginOutput);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignupAsync(UserSignupInput userSignupInput)
        {
            _logger.LogInformation("Signup requested for user {UserEmail}.", userSignupInput.Email);

            await _authService.SignupAsync(userSignupInput);

            return NoContent();
        }
    }
}
