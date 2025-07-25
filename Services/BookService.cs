using LibrarySystem.Middleware.Logs;
using LibrarySystem.Models;
using LibrarySystem.Repositories.Repositories;
using LibrarySystem.Services.Services;

namespace LibrarySystem.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        public async Task Add(Book entity)
        {
            if (entity == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "Book entity cannot be null.", callerType: typeof(BookService));
                throw new ArgumentNullException(nameof(entity), "Book entity cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(entity.Title) || string.IsNullOrWhiteSpace(entity.Author) || string.IsNullOrWhiteSpace(entity.Isbn))
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "Book must have a title, author, and ISBN.", callerType: typeof(BookService));
                throw new ArgumentNullException("Book must have a title, author, and ISBN.");
            }
            await bookRepository.Add(entity);
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, "Book added successfully.", callerType: typeof(BookService));
        }

        public async Task Delete(int id)
        {
            var book =await bookRepository.GetById(id);
            if (book == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, $"Book with ID {id} not found.", callerType: typeof(BookService));
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }
            await bookRepository.Delete(id);
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Book with ID {id} deleted successfully.", callerType: typeof(BookService));
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, "Fetched all books successfully.", callerType: typeof(BookService));
            return await bookRepository.GetAll();
        }

        public async Task<Book> GetById(int id)
        {
            var book = await bookRepository.GetById(id);
            if (book == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, $"Book with ID {id} not found.", callerType: typeof(BookService));
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Fetched book with ID {id} successfully.", callerType: typeof(BookService));
            return book;
        }

        public async Task<IEnumerable<Book>> Search(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "Search string cannot be null or empty.", callerType: typeof(BookService));
                throw new ArgumentNullException("Search string cannot be null or empty.");
            }
             AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Found books with term '{s}'", callerType: typeof(BookService));
            return await bookRepository.Search(s);
        }

        public async Task Update(int id, Book entity)
        {
            if(id != entity.Id)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "The ID in the URL does not match the ID in the book entity.", callerType: typeof(BookService));
                throw new ArgumentException("ID mismatch: The ID in the URL does not match the ID in the book entity.");
            }
            var existingBook = await bookRepository.GetById(id);
            if (existingBook == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, $"Book with ID {id} not found.", callerType: typeof(BookService));
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }
            if (entity == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "Book entity cannot be null.", callerType: typeof(BookService));
                throw new ArgumentException(nameof(entity), "Book entity cannot be null.");
            }
            await bookRepository.Update(entity);
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Book with ID {id} updated successfully.", callerType: typeof(BookService));
        }
    }
}
