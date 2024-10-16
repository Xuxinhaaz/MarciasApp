using System.Linq.Expressions;
using AutoMapper;
using MarciaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarciaApi.Domain.Repository;

public class GenericRepository<T, T2> : IGenericRepository<T, T2> where T : class
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GenericRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<T>> Get(
        int pageNumber, 
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (Expression<Func<T, object>> include in includes)
        {
            query = query.Include(include);
        }

        query = query.Skip(pageNumber * 5).Take(5);
        
        return await query.ToListAsync();
    }

    public async Task<List<T>> Get(
        int pageNumber,
        Expression<Func<T, bool>>? filter = null, 
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if(filter != null) query.Where(filter);
        
        foreach (Expression<Func<T, object>> include in includes)
        {
            query = query.Include(include);
        }

        query = query.Skip(pageNumber * 5).Take(5);

        return await query.ToListAsync();
    }

    public async Task<T> Get(
        Expression<Func<T, bool>> filter, 
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        query = query.Where(filter);
        
        foreach (Expression<Func<T, object>> include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstAsync();
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
    public async Task<bool> Any(Expression<Func<T, bool>> filter)
    {
        return await _context.Set<T>().AnyAsync(filter);
    }

    public async Task Delete(T model)
    {
        _context.Set<T>().Remove(model);
    }

    public async Task<T2> Map(T model)
    {
        return _mapper.Map<T2>(model);
    }

    public async Task<List<T2>> Map(List<T> model)
    {
        return _mapper.Map<List<T2>>(model);
    }

    public async Task Delete(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if(filter != null)
        {
            query = query.Where(filter);
        }

        foreach (Expression<Func<T, object>> include in includes)
        {
            query = query.Include(include);
        }

        _context.Set<T>().Remove(await query.FirstAsync());
    }

    public async Task Delete(List<T> model)
    {
        _context.Set<T>().RemoveRange(model);
    }
}