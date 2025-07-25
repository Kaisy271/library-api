using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class LoginRequestRepository : ILoginRequestRepository
    {
        public readonly LibraryDbContext context;
        public LoginRequestRepository(LibraryDbContext context)
        {
            this.context = context;
        }
        public LoginRequest GetLoginRequestByUsername(string username)
        {
            return context.LoginRequest
                .AsNoTracking()
                .SingleOrDefault(lr => lr.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
        public IEnumerable<LoginRequest> GetAllLoginRequests()
        {
            return context.LoginRequest
                .AsNoTracking()
                .ToList();
        }
    }
   
}
