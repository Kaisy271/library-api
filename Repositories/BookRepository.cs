using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class BookRepository : IBookRepository
    {
        public readonly LibraryDbContext context;
        public BookRepository(LibraryDbContext context)
        {
            this.context = context;
        }
        public async Task Add(Book entity)
        {
            await context.Books.AddAsync(entity);
            await context.SaveChangesAsync() ;
        }

        public async Task Delete(int id)
        {
            var book = context.Books.Find(id);
            context.Books.Remove(book);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await context.Books.AsNoTracking().ToListAsync();
        }

        public async Task<Book> GetById(int id)
        {
            return await context.Books
                .AsNoTracking()
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> Search(string s)
        {
            return await context.Books
                .AsNoTracking()
                .Where(b => b.Title.Contains(s, StringComparison.OrdinalIgnoreCase) ||
                            b.Author.Contains(s, StringComparison.OrdinalIgnoreCase) ||
                            b.Isbn.Contains(s, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task Update(Book entity)
        {
            context.Books.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
