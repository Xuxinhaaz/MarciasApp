namespace MarciaApi.Domain.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public async Task<List<T>> Get(int pageNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetByID(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Any()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Any(string id)
    {
        throw new NotImplementedException();
    }
}