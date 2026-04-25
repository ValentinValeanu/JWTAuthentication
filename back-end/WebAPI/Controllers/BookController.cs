using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Entities;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController(IBookService bookService, ILogger<BookController> logger) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Book>>> GetAsync()
        {
            logger.LogInformation("User gets books.");

            var books = await bookService.GetAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Book>> GetAsync(int id)
        {
            logger.LogInformation($"User gets book with {id}.");

            var book = await bookService.GetAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Book>> CreateAsync(BookInput bookInput)
        {
            logger.LogInformation("User creates book {BookTitle}", bookInput.Title);

            var book = await bookService.CreateAsync(bookInput);

            return Ok(book);
        }
    }
}
