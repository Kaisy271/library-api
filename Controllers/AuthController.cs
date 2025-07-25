using LibrarySystem.Middleware.Logs;
using LibrarySystem.Models;
using LibrarySystem.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly ILoginRequestService loginRequestService;
        public AuthController(IConfiguration config,ILoginRequestService loginRequestService) {
            this.config = config;
            this.loginRequestService = loginRequestService;

        }
        [HttpPost("login")]
        public ActionResult Login( string username, string pass)
        {
            var login = loginRequestService.GetLoginRequestByUsername(username);
            if (login == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Warn, $"Login attempt failed for user {username}: User not found.", callerType: typeof(AuthController));
            }
            if (login.Pass == pass)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"User {username} logged in successfully.", callerType: typeof(AuthController));
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, login.Role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(config["Jwt:ExpireMinutes"])),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Warn, $"Login attempt failed for user {username}: Incorrect password.", callerType: typeof(AuthController));
            }

            return Unauthorized();
        }
    }
}
