using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
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
        return await _context.Set<T>().FindAsync(id) ??
               throw new ProcessException(HttpErrorsCode.NotFound, ErrorMessages.NotFoundError);
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync() ??
               throw new ProcessException(HttpErrorsCode.NotFound, ErrorMessages.NotFoundError);
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
            entity.Init();
            var result = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        catch (Exception exc)
        {
            throw new ProcessException(HttpErrorsCode.InternalServerError, ErrorMessages.SaveEntityError, exc);
        }
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        var result = new List<T>();
        foreach (var entity in entities)
        {
            result.Add(await AddAsync(entity));
        }

        return result;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        try
        {
            entity.Touch();
            var result = _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        catch (Exception exc)
        {
            throw new ProcessException(HttpErrorsCode.InternalServerError, ErrorMessages.UpdateEntityError, exc);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception exc)
        {
            throw new ProcessException(HttpErrorsCode.InternalServerError, ErrorMessages.UpdateEntityError, exc);
        }
    }

    public async Task<bool> Any(Guid entityId)
    {
        return await _context.Set<T>().FindAsync(entityId) is not null;
    }
}