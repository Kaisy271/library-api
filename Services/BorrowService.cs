using LibrarySystem.DTOs;
using LibrarySystem.Middleware.Logs;
using LibrarySystem.Repositories.Repositories;
using LibrarySystem.Services.Services;

namespace LibrarySystem.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository repository;
        public BorrowService(IBorrowRepository repository)
        {
            this.repository = repository;
        }
        public async Task Add(BorrowCreateDTO entity)
        {
            if (entity == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "BorrowCreateDTO cannot be null.", callerType: typeof(BorrowService));
                throw new ArgumentNullException( "BorrowCreateDTO cannot be null.");
            }
            if (entity.BookId == null || entity.UserId == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "Book ID and User ID cannot be null.", callerType: typeof(UserService));
                throw new ArgumentNullException("Book ID and User ID cannot be null.");
            }
            await repository.Add(entity);
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Borrow record created for user {entity.UserId} and book {entity.BookId}.", callerType: typeof(BorrowService));

        }

        public async Task<IEnumerable<BorrowGetDTO>> GetAll()
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, "Fetched all borrow records.", callerType: typeof(BorrowService));
            return await repository.GetAll();
        }

        public async Task<IEnumerable<BorrowGetDTO>> GetByUserId(int id)
        {
            var user = await repository.GetUserById(id);
            if (user == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, $"User with ID {id} not found.", callerType: typeof(BorrowService));
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Fetched borrow records for user {id}.", callerType: typeof(BorrowService));
            return await repository.GetByUserId(id);

        }

        public async Task ReturnBook(int id)
        {
            var record = await repository.GetById(id);
            if (record == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, $"Borrow record with ID {id} not found.", callerType: typeof(BorrowService));
                throw new KeyNotFoundException($"Borrow record with ID {id} not found.");
            }
            await repository.ReturnBook(id);
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Book with borrow record ID {id} returned successfully.", callerType: typeof(BorrowService));
        }
      
    }
}
