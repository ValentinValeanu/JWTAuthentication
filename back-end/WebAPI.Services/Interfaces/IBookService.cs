using WebAPI.Data.Entities;
using WebAPI.Services.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAsync();

        Task<Book?> GetAsync(int id);

        Task<Book> CreateAsync(BookInput book);
    }
}
