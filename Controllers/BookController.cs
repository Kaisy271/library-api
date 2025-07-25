using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using log4net;
using LibrarySystem.Models;
using LibrarySystem.Services.Services;
using LibrarySystem.Middleware.Logs;

namespace LibrarySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;
        private static readonly ILog logger = LogManager.GetLogger(typeof(BookController));

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        // GET: api/book
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            //logger.Info("Fetching all books from the database.");
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Get all books called ", callerType: typeof(BookController));
            var books = await bookService.GetAll();
            return Ok(books);
        }
        // GET: api/book/{id}
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookById(int id)
        {     
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Get book by ID {id} called ", callerType: typeof(BookController));
            var book = await bookService.GetById(id);
            //logger.Info($"Fetching book with ID {id} from the database.");
            return Ok(book);
        }
        // POST: api/book
        [Authorize(Roles = "admin")]

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Create book called ", callerType: typeof(BookController));
            await bookService.Add(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        // PUT: api/book/{id}
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Update book with ID {id} called ", callerType: typeof(BookController));
            await bookService.Update(id, book);
            return NoContent();
        }
        // DELETE: api/book/{id}
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Delete book with ID {id} called ", callerType: typeof(BookController));
            await bookService.Delete(id);
            return NoContent();
        }
        // GET: api/book/search?s={searchTerm}
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(string s)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Search books called ", callerType: typeof(BookController));
            var books = await bookService.Search(s);
            return Ok(books);
        }
    }
}
