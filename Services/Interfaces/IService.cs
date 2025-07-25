namespace LibrarySystem.Services.Services
{
    public interface IService<T>     
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(int id);
        public Task Add(T entity);
        public Task Update(int id,T entity);
        public Task Delete(int id);
    }
}
