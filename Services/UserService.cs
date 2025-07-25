using LibrarySystem.Middleware.Logs;
using LibrarySystem.Models;
using LibrarySystem.Repositories.Repositories;
using LibrarySystem.Services.Services;

namespace LibrarySystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task Add(User entity)
        {
            if (entity == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "User entity cannot be null.", callerType: typeof(UserService));
                throw new ArgumentException(nameof(entity), "User entity cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(entity.Username) || string.IsNullOrWhiteSpace(entity.Email))
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "User must have a username and email.", callerType: typeof(UserService));
                throw new ArgumentException("User must have a username and email.");
            }
            await userRepository.Add(entity);
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"User with username {entity.Username} created successfully.", callerType: typeof(UserService));
        }

        public async Task Delete(int id)
        {
            var user = await userRepository.GetById(id);
            if (user == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, $"User with ID {id} not found.", callerType: typeof(UserService));
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            await userRepository.Delete(id);
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"User with ID {id} deleted successfully.", callerType: typeof(UserService));
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, "Got all users.", callerType: typeof(UserService));
            return await userRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
           
            var user = await userRepository.GetById(id);
            if (user == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, $"User with ID {id} not found.", callerType: typeof(UserService));
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"User with ID {id} retrieved successfully.", callerType: typeof(UserService));
            return user;
        }

        public async Task Update(int id, User entity)
        {
            if(id != entity.Id)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, " The ID in the URL does not match the ID in the user entity.", callerType: typeof(UserService));
                throw new ArgumentException("ID mismatch: The ID in the URL does not match the ID in the user entity.");
            }
            var existingUser = await userRepository.GetById(id);
            if (existingUser == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, $"User with ID {id} not found.", callerType: typeof(UserService));
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            if (entity == null)
            {
                AsyncLogger.Log(Middleware.Logs.LogLevel.Error, "User entity cannot be null", callerType: typeof(UserService));
                throw new ArgumentException(nameof(entity), "User entity cannot be null.");
            }
            await userRepository.Update(entity);
            AsyncLogger.Log(Middleware.Logs.LogLevel.Info, $"Updated user with ID {id}.", callerType: typeof(UserService));

        }
    }
}
