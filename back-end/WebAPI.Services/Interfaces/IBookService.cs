using WebAPI.Data.Entities;
using WebAPI.Services.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> CreateAsync(BookInput book);
    }
}
