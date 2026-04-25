using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Data.Entities;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Services
{
    public class BookService(SandboxContext sandboxContext) : IBookService
    {
        public async Task<IEnumerable<Book>> GetAsync()
        {
            return await sandboxContext.Books.AsNoTracking().ToListAsync();
        }

        public async Task<Book?> GetAsync(int id)
        {
            return await sandboxContext.Books.AsNoTracking()
                                             .FirstOrDefaultAsync(book => book.Id == id);
        }

        public async Task<Book> CreateAsync(BookInput book)
        {
            var bookToCreate = new Book
            {
                Title = book.Title,
                Author = book.Author,
                PageCount = book.PageCount,
                Category = book.Category,
                Edition = book.Edition,
                Language = book.Language,
                LaunchDate = book.LaunchDate,
                TargetGroup = book.TargetGroup
            };

            await sandboxContext.Books.AddAsync(bookToCreate);

            await sandboxContext.SaveChangesAsync();

            return bookToCreate;
        }
    }
}
