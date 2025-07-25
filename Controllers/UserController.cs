using LibrarySystem.Middleware.Logs;
using LibrarySystem.Models;
using LibrarySystem.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace LibrarySystem.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;
        public UserController(IUserService service)
        {
            this.service = service;
        }
        // GET: api/users
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Get all user called ", callerType: typeof(UserController));
            var users = await service.GetAll();
            return Ok(users);
        }

        // GET: api/users/{id}
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Get user by id {id} called ", callerType: typeof(UserController));
            var user = await service.GetById(id);
             return Ok(user);
        }

        // POST: api/users
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
           AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Create user called with username {user.Username}", callerType: typeof(UserController));
            await service.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Update user with id {id} called ", callerType: typeof(UserController));
            await service.Update(id,user);
            return NoContent();
        }

        // DELETE: api/users/{id}
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Delete user with id {id} called ", callerType: typeof(UserController));
            await service.Delete(id);
            return NoContent();
        }
    }
}
