namespace MarciaApi.Domain.Repository;

public interface IGenericRepository<T>
{
    Task<List<T>> Get(int pageNumber);
    Task<T> GetByID(string id);
    Task Add(T model);
    Task SaveAll();
    Task<bool> Any();
}