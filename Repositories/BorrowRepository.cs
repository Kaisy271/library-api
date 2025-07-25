using LibrarySystem.Data;
using LibrarySystem.DTOs;
using LibrarySystem.Models;
using LibrarySystem.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryDbContext context;
        public BorrowRepository (LibraryDbContext context)
        {
            this.context = context;
        }
        public async Task Add(BorrowCreateDTO dto)
        {
            var book = context.Books.SingleOrDefault(b => b.Id == dto.BookId);
            var user = context.Users.SingleOrDefault(u => u.Id == dto.UserId);
            var record = new Borrowrecord
            {
                BookId = dto.BookId,
                UserId = dto.UserId,
                BorrowDate = DateTime.Now
            };

            context.Borrowrecords.Add(record);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BorrowGetDTO>> GetByUserId(int userId)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Id == userId);
                var records = await context.Borrowrecords
                .Where(br => br.UserId == userId)
                .Include(br => br.Book)
                .AsNoTracking()
                .Select(br => new BorrowGetDTO
                {
                    Id = br.Id,
                    BookTitle = br.Book.Title,
                    BookAuthor = br.Book.Author,
                    BookPublishYear = br.Book.PublishYear,
                    BookIsbn = br.Book.Isbn,
                    BorrowDate = br.BorrowDate,
                    ReturnDate = br.ReturnDate,
                    UserFullName = user.Username,
                    UserEmail = user.Email
                })
                .ToListAsync();
                return records; 
        }
        public async Task<User> GetUserById(int userId)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            
        }

        public async Task ReturnBook(int id)
        {
            var record = await context.Borrowrecords.SingleOrDefaultAsync(br => br.Id == id);
            record.ReturnDate = DateTime.Now;
            await context.SaveChangesAsync();
        } 

      

        public async Task<IEnumerable<BorrowGetDTO>> GetAll()
        {
            return await context.Borrowrecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .AsNoTracking()
                .Select(br => new BorrowGetDTO
                {
                    Id = br.Id,
                    BookTitle = br.Book.Title,
                    BookAuthor = br.Book.Author,
                    BookPublishYear = br.Book.PublishYear,
                    BookIsbn = br.Book.Isbn,
                    BorrowDate = br.BorrowDate,
                    ReturnDate = br.ReturnDate,
                    UserFullName = br.User.Username,
                    UserEmail = br.User.Email
                })
                .ToListAsync();
        }

        public async Task<Borrowrecord> GetById(int id)
        {
           return await context.Borrowrecords
                .AsNoTracking()
                .SingleOrDefaultAsync(br => br.Id == id);
        }
    }
}
