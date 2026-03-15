using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginDTO userLoginDTO)
        {
            await _authService.LoginAsync(userLoginDTO);

            return NoContent();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignupAsync(UserSignupDTO userSignupDTO)
        {
            await _authService.SignupAsync(userSignupDTO);

            return NoContent();
        }
    }
}
