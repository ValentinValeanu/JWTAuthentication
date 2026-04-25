using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserOutput>>> GetAsync()
        {
            var usersOutput = await userService.GetOutputsAsync();

            return Ok(usersOutput);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserOutput>> GetAsync(int id)
        {
            // the user can only access his data, check ID inside access token
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null || !int.TryParse(userIdFromToken, out var intUserIdFromToken))
            {
                return Unauthorized();
            }

            var user = await userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            else if (user.Id != intUserIdFromToken)
            {
                return Forbid();
            }

            return Ok(new UserOutput(user.FirstName, user.LastName, user.Email, user.BirthDate));
        }
    }
}
