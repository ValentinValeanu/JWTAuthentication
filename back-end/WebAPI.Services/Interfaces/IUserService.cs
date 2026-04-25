using WebAPI.Data.Entities;
using WebAPI.Services.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserOutput>> GetOutputsAsync();

        Task<User?> GetUserAsync(int id);

        Task<UserOutput?> GetOutputAsync(int id);
    }
}
