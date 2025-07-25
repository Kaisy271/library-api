using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext context;
        public UserRepository(LibraryDbContext context)
        {
            this.context = context;
        }

        public async Task Add(User entity)
        {
            context.Users.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = context.Users.SingleOrDefault(u => u.Id == id);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task Update(User entity)
        {
            var user = context.Users.SingleOrDefault(u => u.Id == entity.Id);
            user.Username = entity.Username;
            user.Email = entity.Email;
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
