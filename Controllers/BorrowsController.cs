using LibrarySystem.DTOs;
using LibrarySystem.Middleware.Logs;
using LibrarySystem.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowsController : ControllerBase
    {
        private readonly IBorrowService service;
        public BorrowsController(IBorrowService service)
        {
            this.service = service;
        }
        // POST: /api/borrow
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBorrow([FromBody] BorrowCreateDTO dto)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Create borrow record called for user {dto.UserId} and book {dto.BookId}", callerType: typeof(BorrowsController));
            await service.Add(dto);
            return CreatedAtAction(nameof(GetBorrowsByUserId), new { userId = dto.UserId }, dto);
        }

        // PUT: /api/borrow/{id}/return
        [Authorize]
        [HttpPut("{id}/return")]
        public async Task<ActionResult> ReturnBook(int id)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Return book with borrow record id {id} called", callerType: typeof(BorrowsController));
            await service.ReturnBook(id);
            return NoContent();
        }

        // GET: /api/borrow
        [Authorize(Roles ="admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBorrows()
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, "Get all borrow records called", callerType: typeof(BorrowsController));
            var records = await service.GetAll();
            return Ok(records);
        }

        // GET: /api/borrow/user/{userId}
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBorrowsByUserId(int userId)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Get borrow records for user {userId} called", callerType: typeof(BorrowsController));
            var book = await service.GetByUserId(userId);
                return Ok(book);
        }
    }
}
