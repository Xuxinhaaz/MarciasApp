using MarciaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarciaApi.Domain.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<T>> Get(int pageNumber)
    {
        return await _context.Set<T>().Skip(pageNumber * 5).Take(5).ToListAsync();
    }

    public async Task<T> GetByID(string id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task Add(T model)
    {
        await _context.Set<T>().AddAsync(model);
    }

    public async Task SaveAll()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Any()
    {
        return await _context.Set<T>().AnyAsync();
    }

    
}