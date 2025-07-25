using LibrarySystem.Models;

namespace LibrarySystem.Repositories.Interfaces
{
    public interface ILoginRequestRepository
    {
        public LoginRequest GetLoginRequestByUsername(string username);
        public IEnumerable<LoginRequest> GetAllLoginRequests();
    }
}
