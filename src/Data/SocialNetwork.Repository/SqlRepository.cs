using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Context;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.Repository;

public class SqlRepository<T> : IRepository<T> where T : class, IBaseEntity
{
    private readonly MainDbContext _context;

    public SqlRepository(MainDbContext context)
    {
        _context = context;
    }

    public async Task<T> GetAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id) ?? throw new ProcessException(ErrorMessages.NotFoundError);
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
        try
        {
            var result = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        catch (Exception exc)
        {
            throw new ProcessException(ErrorMessages.SaveEntityError, exc);
        }
    }

    public async Task<T> UpdateAsync(T entity)
    {
        try
        {
            entity.ModificationDateTime = DateTimeOffset.Now;
            var result = _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        catch (Exception exc)
        {
            throw new ProcessException(ErrorMessages.UpdateEntityError, exc);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}