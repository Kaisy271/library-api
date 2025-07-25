using LibrarySystem.DTOs;

namespace LibrarySystem.Services.Services
{
    public interface IBorrowService
    {
        public Task<IEnumerable<BorrowGetDTO>> GetAll();
        public Task<IEnumerable<BorrowGetDTO>> GetByUserId(int id);
        public Task Add(BorrowCreateDTO entity);
        public Task ReturnBook(int id);
    }
}
