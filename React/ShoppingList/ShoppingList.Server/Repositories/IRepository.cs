namespace ShoppingList.Server.Repositories;

public interface IRepository<T>
{
    public Task<T> AddAsync(T entity);
    public Task<List<T>> GetAllAsync();
    public Task<T?> GetOneAsync(int id);
}