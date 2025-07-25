using LibrarySystem.DTOs;
using LibrarySystem.Models;

namespace LibrarySystem.Repositories.Repositories
{
    public interface IBorrowRepository 
    {
        public Task<IEnumerable<BorrowGetDTO>> GetAll();
        public Task<IEnumerable<BorrowGetDTO>> GetByUserId(int id);
        public Task<Borrowrecord> GetById(int id);
        public Task<User> GetUserById(int userId);
        public Task Add(BorrowCreateDTO entity);
        public Task ReturnBook( int id);
    }
}
