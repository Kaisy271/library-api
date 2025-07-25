using LibrarySystem.Models;

namespace LibrarySystem.Services.Services
{
    public interface IBookService : IService<Book>
    {
        public Task<IEnumerable<Book>> Search(string s);
    }
}
