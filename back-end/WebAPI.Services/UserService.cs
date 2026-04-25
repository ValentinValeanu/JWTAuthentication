using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Data.Entities;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Services
{
    public class UserService(SandboxContext sandboxContext) : IUserService
    {
        public async Task<IEnumerable<UserOutput>> GetOutputsAsync()
        {
            return await sandboxContext.Users.AsNoTracking()
                                             .Select(user => new UserOutput(
                                                                user.FirstName, 
                                                                user.LastName, 
                                                                user.Email, 
                                                                user.BirthDate)
                                             ).ToListAsync();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await sandboxContext.Users.AsNoTracking()
                                             .Include(user => user.Roles)
                                             .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<UserOutput?> GetOutputAsync(int id)
        {
            var user = await this.GetUserAsync(id);

            if (user == null)
            {
                return null;
            }

            return new UserOutput(user.FirstName, user.LastName, user.Email, user.BirthDate);
        }
    }
}
