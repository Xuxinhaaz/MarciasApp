using System.Linq.Expressions;

namespace MarciaApi.Domain.Repository;

public interface IGenericRepository<T, T2>
{
    Task<List<T>> Get(int pageNumber, params Expression<Func<T, object>>[] includes);
    Task<List<T>> Get(string id, int pageNumber, Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
    Task<T> GetByID(string id, Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
    Task Add(T model);
    Task SaveAll();
    Task<bool> Any();
    Task<bool> Any(Expression<Func<T, bool>> filter);
    Task Delete(T model);
    Task<T2> Map(T model);
    Task<List<T2>> Map(List<T> model);
    Task DeleteAnEntity(T model);
    Task DeleteEntities(List<T> model);
}