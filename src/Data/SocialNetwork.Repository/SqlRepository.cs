using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.Repository;

// TODO обработка исключений
public class SqlRepository<T> : IRepository<T> where T : class, IBaseEntity
{
    private readonly MainDbContext _context;

    public SqlRepository(MainDbContext context)
    {
        _context = context;
    }

    public async Task<T> GetAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync(int offset = 0, int limit = 10)
    {
        return await _context.Set<T>()
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)))
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, int offset = 0, int limit = 10)
    {
        return await _context.Set<T>()
            .Where(predicate)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)))
            .ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        var result = await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        entity.ModificationDateTime = DateTime.Now;
        var result = _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}