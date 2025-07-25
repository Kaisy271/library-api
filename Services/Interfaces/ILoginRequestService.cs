using LibrarySystem.Models;

namespace LibrarySystem.Services.Interfaces
{
    public interface ILoginRequestService
    {
        public LoginRequest GetLoginRequestByUsername(string username);
        public IEnumerable<LoginRequest> GetAllLoginRequests();
    }
}
