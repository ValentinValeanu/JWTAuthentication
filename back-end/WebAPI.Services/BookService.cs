using WebAPI.Data;
using WebAPI.Data.Entities;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Services
{
    public class BookService(SandboxContext sandboxContext) : IBookService
    {
        private readonly SandboxContext _sandboxContext = sandboxContext;

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

            await _sandboxContext.Books.AddAsync(bookToCreate);

            await _sandboxContext.SaveChangesAsync();

            return bookToCreate;
        }
    }
}
