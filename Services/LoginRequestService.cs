using LibrarySystem.Models;
using LibrarySystem.Repositories.Interfaces;
using LibrarySystem.Services.Interfaces;

namespace LibrarySystem.Services
{
    public class LoginRequestService : ILoginRequestService
    {
        private readonly ILoginRequestRepository loginRequestRepository;
        public LoginRequestService(ILoginRequestRepository loginRequestRepository)
        {
            this.loginRequestRepository = loginRequestRepository;
        }
        public LoginRequest GetLoginRequestByUsername(string username)
        {
            if(string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }
            return loginRequestRepository.GetLoginRequestByUsername(username);
        }
        public IEnumerable<LoginRequest> GetAllLoginRequests()
        {
            return loginRequestRepository.GetAllLoginRequests();
        }
    }
    
}
