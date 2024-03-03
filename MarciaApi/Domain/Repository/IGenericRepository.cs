namespace MarciaApi.Domain.Repository;

public interface IGenericRepository<T>
{
    Task<List<T>> Get(int pageNumber);
    Task<T> GetByID(string id);
    Task<bool> Any();
    Task<bool> Any(string id);
}