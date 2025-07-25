using LibrarySystem.Models;

namespace LibrarySystem.Repositories.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        public Task<IEnumerable<Book>> Search(string s);
    }
}
