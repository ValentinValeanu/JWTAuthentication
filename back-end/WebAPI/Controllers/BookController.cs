using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Interfaces;
using WebAPI.Services.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController(IBookService bookService, ILogger<BookController> logger) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;
        private readonly ILogger<BookController> _logger = logger;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(BookInput bookInput)
        {
            _logger.LogInformation("User creates book {BookTitle}", bookInput.Title);

            var book = await _bookService.CreateAsync(bookInput);

            return Ok(book);
        }
    }
}
